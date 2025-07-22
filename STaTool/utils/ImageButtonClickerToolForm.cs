using log4net;
using STaTool.plc;
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

        private string _plcIp;
        private int _plcPort;
        private volatile bool _plcFlagDetected = false;
        private const int _checkDelay = 500;

        public ImageButtonClickerToolForm(List<string> imageButtons, string plcIp, int plcPort, Action resetControls) {
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
            if (config.RepeatTimes != 0) {
                textBox_repeat_times.Text = config.RepeatTimes.ToString();
            }
            if (config.ClickInterval != 0) {
                textBox_click_interval.Text = config.ClickInterval.ToString();
            }

            _imageButtons = imageButtons;
            _plcIp = plcIp;
            _plcPort = plcPort;
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
            string repeatTimesStr = textBox_repeat_times.Text;
            string clickIntervalStr = textBox_click_interval.Text;
            string checkIntervalStr = textBox_check_interval.Text;
            //string checkIntervalStr = text
            if (string.IsNullOrWhiteSpace(repeatTimesStr) || string.IsNullOrWhiteSpace(clickIntervalStr)
                || string.IsNullOrWhiteSpace(checkIntervalStr)) {
                WidgetUtils.ShowWarningPopUp("请填入所有必须信息！");
                return;
            }

            // Create modbus client
            var fx5UModbusClient = new Fx5uModbusClient();
            try {
                fx5UModbusClient.Connect(_plcIp, _plcPort);
                WidgetUtils.AppendMsg("PLC连接成功！");
            } catch (Exception ex) {
                WidgetUtils.ShowWarningPopUp($"无法使用【ip={_plcIp}，port={_plcPort}】连接到PLC！");
                log.Warn($"无法使用【ip={_plcIp}，port={_plcPort}】连接到PLC！", ex);
                return;
            }

            int repeatTimes = int.Parse(repeatTimesStr);
            int clickInterval = int.Parse(clickIntervalStr);
            int checkInterval = int.Parse(checkIntervalStr);

            // Update config
            Config config = FileUtil.LoadConfig();
            config.RepeatTimes = repeatTimes;
            config.ClickInterval = clickInterval;
            config.CheckInterval = checkInterval;
            FileUtil.SaveConfig(config);

            button_start_fetch.Enabled = false;
            button_stop_fetch.Enabled = true;

            _cts = new CancellationTokenSource();
            CancellationToken token = _cts.Token;
            fetchingCurveData = true;

            Task.Run(async () => {
                WidgetUtils.AppendMsg("开始自动抓取曲线数据...");
                log.Info("Start fetching curve data...");

                var plcService = new PlcPollingService(fx5UModbusClient);
                var clicker = new ImageRecognitionClicker();

                if (!clicker.InitOk) {
                    WidgetUtils.AppendMsg("图像识别初始化失败，请检查截图是否正确！");
                    return;
                }

                try {
                    // 启动 PLC 轮询服务
                    _ = plcService.StartPollingAsync(
                        checkInterval,
                        config.PlcFlagPos,
                        config.PlcHeartBeatPos,
                        flag => {
                            _plcFlagDetected = flag;
                            if (flag) {
                                WidgetUtils.AppendMsg("收到PLC抓取曲线指令，正在进行曲线抓取...");
                            }
                        },
                        token);

                    // 主循环
                    while (!token.IsCancellationRequested) {
                        if (_plcFlagDetected) {
                            int count = 0;
                            while (count < repeatTimes && !token.IsCancellationRequested) {
                                for (int i = 0; i < _imageButtons.Count; i++) {
                                    token.ThrowIfCancellationRequested();

                                    string btnImg = FileUtil.GetImagePath(_imageButtons[i]);
                                    if (i == 1) {
                                        clicker.ClickButtonByImage_Special(btnImg);
                                    } else {
                                        clicker.ClickButtonByImage(btnImg);
                                    }

                                    await Task.Delay(clickInterval, token);
                                }

                                count++;
                                WidgetUtils.AppendMsg($"第 {count} 次曲线抓取完成！");
                            }

                            // 重置 PLC 标志位
                            fx5UModbusClient.WriteRegister(config.PlcFlagPos, 0);
                            WidgetUtils.AppendMsg("曲线抓取全部完成！已重置PLC标识。");
                        }

                        await Task.Delay(_checkDelay, token);
                    }
                } catch (TaskCanceledException) {
                    WidgetUtils.AppendMsg("曲线抓取已取消。");
                } catch (Exception ex) {
                    log.Error($"曲线抓取过程中发生异常: {ex.Message}", ex);
                    WidgetUtils.AppendMsg($"发生错误: {ex.Message}");
                } finally {
                    // 确保资源释放
                    fx5UModbusClient.WriteRegister(config.PlcFlagPos, 0);
                    fx5UModbusClient.Disconnect();
                    WidgetUtils.AppendMsg("PLC连接已断开...");

                    fetchingCurveData = false;

                    button_start_fetch.Enabled = true;
                    button_stop_fetch.Enabled = false;
                }
            }, token); // 将 token 传递到最外层的 Task.Run
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
