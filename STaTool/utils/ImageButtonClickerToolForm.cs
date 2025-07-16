using log4net;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace STaTool.utils {
    public partial class ImageButtonClickerToolForm: Form {
        private ILog log;

        // 钩子句柄
        private IntPtr _hookID = IntPtr.Zero;
        private LowLevelKeyboardProc _proc;

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private volatile bool fetchingCurveData = false;

        private List<string> _imageButtons;
        private Action _resetControls;

        public ImageButtonClickerToolForm(List<string> imageButtons, Action resetControls) {
            log = LogManager.GetLogger(GetType());

            _proc = HookCallback;
            _hookID = SetHook(_proc);

            InitializeComponent();

            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            Location = new Point(
                workingArea.Right - Width,
                workingArea.Bottom - Height
            );
            Config config = FileUtil.LoadConfig();
            if (config.FetchInterval != 0) {
                textBox_fetch_interval.Text = config.FetchInterval.ToString();
            }
            if (config.ClickInterval != 0) {
                textBox_click_interval.Text = config.ClickInterval.ToString();
            }

            _imageButtons = imageButtons;
            _resetControls = resetControls;

            button_start_fetch.Click += ButtonStartFetch_Click;
            button_stop_fetch.Click += ButtonStopFetch_Click;
        }

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);

            if (fetchingCurveData) {
                if (WidgetUtils.ShowConfirmPopUp("曲线数据抓取中，确定关闭？")) {
                    StopFetch();
                    log.Info("Manually close fetching tool window.");
                } else {
                    e.Cancel = true;
                }
            }
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            button_start_fetch.PerformClick();
        }

        protected override void OnHandleDestroyed(EventArgs e) {
            _resetControls();
            StopFetch();
            base.OnHandleDestroyed(e);
        }

        private void ButtonStartFetch_Click(object? sender, EventArgs e) {
            string fetchIntervalStr = textBox_fetch_interval.Text;
            string clickIntervalStr = textBox_click_interval.Text;
            if (string.IsNullOrWhiteSpace(fetchIntervalStr)
                || string.IsNullOrWhiteSpace(clickIntervalStr)) {
                WidgetUtils.ShowWarningPopUp("间隔时间不能为空！");
                return;
            }

            int fetchInterval = int.Parse(fetchIntervalStr);
            int clickInterval = int.Parse(clickIntervalStr);

            // Update config
            Config config = FileUtil.LoadConfig();
            config.FetchInterval = fetchInterval;
            config.ClickInterval = clickInterval;
            FileUtil.SaveConfig(config);

            button_start_fetch.Enabled = false;
            button_stop_fetch.Enabled = true;

            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;
            fetchingCurveData = true;

            Task.Run(async () => {
                WidgetUtils.AppendMsg("开始自动抓取曲线数据...");
                log.Info("Start fetching curve data...");

                var clicker = new ImageRecognitionClicker();
                if (clicker.InitOk) {
                    try {
                        while (!token.IsCancellationRequested) {
                            for (int i = 0; i < _imageButtons.Count; i++) {
                                string btnImg = FileUtil.GetImagePath(_imageButtons[i]);
                                if (i == 1) {
                                    clicker.ClickButtonByImage_Special(btnImg);
                                } else {
                                    clicker.ClickButtonByImage(btnImg);
                                }

                                if (i != _imageButtons.Count - 1) {
                                    await Task.Delay(clickInterval, token);
                                } else {
                                    await Task.Delay(fetchInterval, token);
                                }
                            }
                        }
                    } catch (TaskCanceledException) {
                    }
                }
            });
        }

        private void ButtonStopFetch_Click(object? sender, EventArgs e) {
            StopFetch();
        }

        private void StopFetch() {
            WidgetUtils.AppendMsg("停止抓取曲线数据...");
            log.Info("Stop fetching curve data...");

            _cts.Cancel();
            fetchingCurveData = false;

            button_start_fetch.Enabled = true;
            button_stop_fetch.Enabled = false;
        }

        // 设置钩子
        private IntPtr SetHook(LowLevelKeyboardProc proc) {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // 卸载钩子
        protected override void OnFormClosed(FormClosedEventArgs e) {
            UnhookWindowsHookEx(_hookID);
            base.OnFormClosed(e);
        }

        // 回调函数
        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0 && (wParam == (IntPtr) WM_KEYDOWN)) {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys) vkCode;

                if (fetchingCurveData) {
                    if (key == Keys.F10) {
                        StopFetch();
                        return (IntPtr) 1;
                    }
                }

                // 如果你想阻止这个按键传递给系统，返回非零值
                // return (IntPtr)1;
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        // 钩子类型和 API 定义
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
