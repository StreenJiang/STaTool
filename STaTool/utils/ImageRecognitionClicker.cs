using log4net;
using OpenCvSharp;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using WindowsInput;

namespace STaTool.utils {

    public class ImageRecognitionClicker {
        private ILog log;
        private readonly InputSimulator _inputSimulator = new InputSimulator();

        public bool InitOk { get; set; } = true;

        public ImageRecognitionClicker() {
            log = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// 通过图像识别点击按钮
        /// </summary>
        /// <param name="templateImagePath">按钮模板图片路径(PNG)</param>
        /// <param name="threshold">匹配阈值(0-1)</param>
        /// <returns>是否成功点击</returns>
        public bool ClickButtonByImage(string templateImagePath, double threshold = 0.9) {
            try {
                // 1. 截取屏幕并确保格式正确
                using var screenCapture = CaptureScreen();

                // 2. 加载模板图像并确保格式正确
                using var templateImage = new Mat(templateImagePath, ImreadModes.Color);
                if (templateImage.Empty()) {
                    log.Warn($"Cannot load image template, template [{templateImagePath}] is null.");
                    return false;
                }

                // 3. 转换为相同颜色空间
                using var screenMat = new Mat();
                if (screenCapture.Channels() == 3)
                    Cv2.CvtColor(screenCapture, screenMat, ColorConversionCodes.BGR2BGRA);
                else
                    screenCapture.CopyTo(screenMat);

                using var templateMat = new Mat();
                if (templateImage.Channels() == 3)
                    Cv2.CvtColor(templateImage, templateMat, ColorConversionCodes.BGR2BGRA);
                else
                    templateImage.CopyTo(templateMat);

                // 4. 图像匹配
                var matchResult = MatchTemplate(screenMat, templateMat, threshold);

                if (!matchResult.IsMatchFound) {
                    log.Warn($"Cannot find matched button with image template [{templateImagePath}].");
                    return false;
                }

                // 5. 计算中心坐标并点击
                var centerX = matchResult.Location.X + (templateMat.Width / 2);
                var centerY = matchResult.Location.Y + (templateMat.Height / 2);

                ClickAt(centerX, centerY);

                return true;
            } catch (Exception ex) {
                WidgetUtils.ShowErrorPopUp($"图像【{templateImagePath}】识别点击失败: {ex.Message}");
                log.Error($"Matching button with image tempalte [{templateImagePath}] failed.", ex);
                return false;
            }
        }

        public bool ClickButtonByImage_Special(string templateImagePath, double threshold = 0.9) {
            try {
                // 1. 截取屏幕并确保格式正确
                using var screenCapture = CaptureScreen();

                // 2. 加载模板图像并确保格式正确
                using var templateImage = new Mat(templateImagePath, ImreadModes.Color);
                if (templateImage.Empty()) {
                    log.Warn($"Cannot load image template, template [{templateImagePath}] is null.");
                    return false;
                }

                // 3. 转换为相同颜色空间
                using var screenMat = new Mat();
                if (screenCapture.Channels() == 3)
                    Cv2.CvtColor(screenCapture, screenMat, ColorConversionCodes.BGR2BGRA);
                else
                    screenCapture.CopyTo(screenMat);

                using var templateMat = new Mat();
                if (templateImage.Channels() == 3)
                    Cv2.CvtColor(templateImage, templateMat, ColorConversionCodes.BGR2BGRA);
                else
                    templateImage.CopyTo(templateMat);

                // 4. 图像匹配
                var matchResult = MatchTemplate(screenMat, templateMat, threshold);

                if (!matchResult.IsMatchFound) {
                    log.Warn($"Cannot find matched button with image template [{templateImagePath}].");
                    return false;
                }

                // 5. 计算中心坐标并点击
                var centerX = matchResult.Location.X + (templateMat.Width / 2);
                var centerY = matchResult.Location.Y + (int) Math.Abs((templateMat.Height * 1.5));

                ClickAt(centerX, centerY);

                return true;
            } catch (Exception ex) {
                WidgetUtils.ShowErrorPopUp($"图像【{templateImagePath}】识别点击失败: {ex.Message}");
                log.Error($"Matching button with image tempalte [{templateImagePath}] failed.", ex);
                return false;
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
