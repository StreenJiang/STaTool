using log4net;
using OpenCvSharp;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using WindowsInput;
using WindowsInput.Native;

namespace STaTool.utils {

    public class ImageRecognitionClicker {
        private ILog log;
        private readonly InputSimulator _inputSimulator = new InputSimulator();
        private const int RETRY_DELAY = 100;
        private const int LOCATE_RETRIES = 3;
        private const int CLICK_ROUNDS = 3;

        public bool InitOk { get; set; } = true;

        public ImageRecognitionClicker() {
            log = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// 通过图像识别点击按钮。点击前定位失败按 100ms 间隔重试，最多 3 次；
        /// 点击后再次检测，图片消失视为点击成功，仍可见则重试点击，最多 3 轮。
        /// </summary>
        /// <param name="templateImagePath">按钮模板图片路径(PNG)</param>
        /// <param name="threshold">匹配阈值(0-1)</param>
        /// <returns>是否成功点击</returns>
        public async Task<bool> ClickButtonByImage(string templateImagePath, double threshold = 0.9) {
            using var templateMat = LoadTemplate(templateImagePath);
            if (templateMat == null) {
                return false;
            }

            for (int loop = 1; loop <= CLICK_ROUNDS; loop++) {
                // 点击前定位：最多重试 3 次
                if (!await ClickOnce(templateMat, templateImagePath, threshold, clickBelow: false)) {
                    return false;
                }

                // 点击后再次检测，图片消失视为点击成功
                await Task.Delay(RETRY_DELAY);
                if (FindTemplate(templateMat, templateImagePath, threshold, clickBelow: false) == null) {
                    return true;
                }

                if (loop < CLICK_ROUNDS) {
                    log.Warn($"Button [{templateImagePath}] is still visible after click, retrying ({loop}/{CLICK_ROUNDS}).");
                }
            }

            log.Warn($"Button [{templateImagePath}] is still visible after {CLICK_ROUNDS} click attempts.");
            return false;
        }

        /// <summary>
        /// 通过图像识别点击模板下方区域（曲线表头专用）。
        /// 点击前定位失败按 100ms 间隔重试，最多 3 次；不做点击后校验。
        /// </summary>
        /// <param name="templateImagePath">按钮模板图片路径(PNG)</param>
        /// <param name="threshold">匹配阈值(0-1)</param>
        /// <returns>是否成功点击</returns>
        public async Task<bool> ClickButtonByImage_Special(string templateImagePath, double threshold = 0.9) {
            using var templateMat = LoadTemplate(templateImagePath);
            if (templateMat == null) {
                return false;
            }
            return await ClickOnce(templateMat, templateImagePath, threshold, clickBelow: true);
        }

        /// <summary>
        /// 将「另存为」对话框导航到指定目录：Ctrl+L 聚焦顶部地址栏，输入路径后回车。
        /// </summary>
        /// <param name="directoryPath">目标目录路径</param>
        public void NavigateSaveDialogTo(string directoryPath) {
            // 1. 聚焦顶部地址栏
            _inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_L);
            _inputSimulator.Keyboard.Sleep(150);

            // 2. 全选并输入目录路径（路径可能含中文，使用 Unicode 输入）
            _inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
            GlobalInputSimulator.SendUnicodeText(directoryPath);
            _inputSimulator.Keyboard.Sleep(150);

            // 3. 回车导航到该目录
            _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
            _inputSimulator.Keyboard.Sleep(300);
        }

        /// <summary>
        /// 加载并转换模板图像，加载失败返回 null。
        /// </summary>
        private Mat? LoadTemplate(string templateImagePath) {
            var templateImage = new Mat(templateImagePath, ImreadModes.Color);
            if (templateImage.Empty()) {
                templateImage.Dispose();
                log.Warn($"Cannot load image template, template [{templateImagePath}] is null.");
                return null;
            }

            var templateMat = new Mat();
            if (templateImage.Channels() == 3)
                Cv2.CvtColor(templateImage, templateMat, ColorConversionCodes.BGR2BGRA);
            else
                templateImage.CopyTo(templateMat);

            templateImage.Dispose();
            return templateMat;
        }

        /// <summary>
        /// 定位并点击一次，定位失败按 100ms 间隔重试，最多 3 次。
        /// </summary>
        private async Task<bool> ClickOnce(Mat templateMat, string templateImagePath, double threshold, bool clickBelow) {
            for (int attempt = 1; attempt <= LOCATE_RETRIES; attempt++) {
                var point = FindTemplate(templateMat, templateImagePath, threshold, clickBelow);
                if (point.HasValue) {
                    ClickAt(point.Value.X, point.Value.Y);
                    return true;
                }

                if (attempt < LOCATE_RETRIES) {
                    await Task.Delay(RETRY_DELAY);
                }
            }

            log.Warn($"Cannot find matched button with image template [{templateImagePath}] after {LOCATE_RETRIES} attempts.");
            return false;
        }

        /// <summary>
        /// 截屏并进行一次模板匹配，返回点击坐标；未匹配到返回 null。
        /// </summary>
        private System.Drawing.Point? FindTemplate(Mat templateMat, string templateImagePath, double threshold, bool clickBelow) {
            try {
                // CaptureScreen 始终返回 8UC4，与转换后的模板类型一致
                using var screenMat = CaptureScreen();

                var matchResult = MatchTemplate(screenMat, templateMat, threshold);
                if (!matchResult.IsMatchFound) {
                    return null;
                }

                return new System.Drawing.Point(
                    matchResult.Location.X + (templateMat.Width / 2),
                    clickBelow
                        ? matchResult.Location.Y + (int) Math.Abs(templateMat.Height * 1.5)
                        : matchResult.Location.Y + (templateMat.Height / 2)
                );
            } catch (Exception ex) {
                // 记录异常，由调用方决定是否重试
                log.Warn($"Attempt to match button with image template [{templateImagePath}] failed at {DateTime.Now:HH:mm:ss.fff}. Error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 截取屏幕图像
        /// </summary>
        private Mat CaptureScreen() {
            var bounds = Screen.PrimaryScreen.Bounds;
            using var bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(bitmap)) {
                graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
            }

            return BitmapToMat(bitmap);
        }

        /// <summary>
        /// 将Bitmap转换为Mat
        /// </summary>
        private Mat BitmapToMat(Bitmap bitmap) {
            // 确保转换为 32bppArgb 格式
            var convertedBitmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(convertedBitmap)) {
                g.DrawImage(bitmap, 0, 0);
            }

            var bitmapData = convertedBitmap.LockBits(
                new Rectangle(0, 0, convertedBitmap.Width, convertedBitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            try {
                return Mat.FromPixelData(convertedBitmap.Height, convertedBitmap.Width,
                        MatType.CV_8UC4,
                        bitmapData.Scan0,
                        bitmapData.Stride);
            } finally {
                convertedBitmap.UnlockBits(bitmapData);
            }
        }

        /// <summary>
        /// 模板匹配
        /// </summary>
        private (bool IsMatchFound, OpenCvSharp.Point Location) MatchTemplate(Mat source, Mat template, double threshold) {
            // 确保图像类型一致
            if (source.Type() != template.Type()) {
                // 将模板图像转换为源图像类型
                using var convertedTemplate = new Mat();
                template.ConvertTo(convertedTemplate, source.Type());
                template = convertedTemplate;
            }

            using var result = new Mat();
            Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed);

            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            return (maxVal >= threshold, maxLoc);
        }

        /// <summary>
        /// 在指定坐标模拟鼠标点击
        /// </summary>
        private void ClickAt(int x, int y) {
            // 移动鼠标到目标位置
            SetCursorPos(x, y);

            // 模拟鼠标点击
            //GlobalInputSimulator.GlobalClick(x, y);
            _inputSimulator.Mouse
                .LeftButtonDown()
                .Sleep(50)
                .LeftButtonUp();
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
    }
}
