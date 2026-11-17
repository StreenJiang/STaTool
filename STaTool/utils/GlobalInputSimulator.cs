using System.Diagnostics;
using System.Runtime.InteropServices;

namespace STaTool.utils {
    public class GlobalInputSimulator {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentThreadId();

        const int SW_RESTORE = 9;

        /// <summary>
        /// 全局点击（可点击任何窗口）
        /// </summary>
        public static void GlobalClick(int x, int y, string targetProcessName = null) {
            IntPtr originalForeground = GetForegroundWindow();

            try {
                if (!string.IsNullOrEmpty(targetProcessName)) {
                    // 获取目标窗口句柄
                    var targetWindow = GetProcessWindow(targetProcessName);
                    if (targetWindow != IntPtr.Zero) {
                        // 切换到目标窗口
                        ForceForegroundWindow(targetWindow);
                        Thread.Sleep(100); // 等待窗口激活
                    }
                }

                // 设置鼠标位置
                Cursor.Position = new Point(x, y);

                // 使用更底层的输入模拟
                MouseEvent(MouseEventFlags.LeftDown, x, y);
                Thread.Sleep(50);
                MouseEvent(MouseEventFlags.LeftUp, x, y);
            } finally {
                // 恢复原始窗口焦点
                if (originalForeground != IntPtr.Zero) {
                    SetForegroundWindow(originalForeground);
                }
            }
        }

        private static IntPtr GetProcessWindow(string processName) {
            var processes = Process.GetProcessesByName(processName);
            return processes.Length > 0 ? processes[0].MainWindowHandle : IntPtr.Zero;
        }

        private static void ForceForegroundWindow(IntPtr hWnd) {
            uint currentThread = GetCurrentThreadId();
            uint targetThread = GetWindowThreadProcessId(hWnd, IntPtr.Zero);

            // 附加线程输入
            if (currentThread != targetThread)
                AttachThreadInput(currentThread, targetThread, true);

            ShowWindow(hWnd, SW_RESTORE);
            SetForegroundWindow(hWnd);

            // 分离线程输入
            if (currentThread != targetThread)
                AttachThreadInput(currentThread, targetThread, false);
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

        [Flags]
        private enum MouseEventFlags {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            Absolute = 0x00008000
        }

        private static void MouseEvent(MouseEventFlags flags, int x, int y) {
            // 转换为绝对坐标
            int absX = (x * 65535) / Screen.PrimaryScreen.Bounds.Width;
            int absY = (y * 65535) / Screen.PrimaryScreen.Bounds.Height;

            mouse_event((uint) (flags | MouseEventFlags.Absolute), (uint) absX, (uint) absY, 0, IntPtr.Zero);
        }

        /// <summary>
        /// 以 Unicode 方式输入字符串，兼容中文路径，不受输入法状态影响。
        /// </summary>
        public static void SendUnicodeText(string text) {
            var inputs = new INPUT[text.Length * 2];
            for (int i = 0; i < text.Length; i++) {
                inputs[i * 2] = new INPUT {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion {
                        ki = new KEYBDINPUT {
                            wScan = text[i],
                            dwFlags = KEYEVENTF_UNICODE
                        }
                    }
                };
                inputs[i * 2 + 1] = new INPUT {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion {
                        ki = new KEYBDINPUT {
                            wScan = text[i],
                            dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP
                        }
                    }
                };
            }

            SendInput((uint) inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_UNICODE = 0x0004;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT {
            public uint type;
            public InputUnion U;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }
    }
}
