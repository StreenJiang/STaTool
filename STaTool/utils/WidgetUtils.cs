namespace STaTool.utils {
    public class WidgetUtils {
        private static readonly ErrorProvider errorProvider;
        private static readonly ToolTip toolTip;
        public static TextBox? TextBox_realtime_log { get; set; }

        static WidgetUtils() {
            errorProvider = new ErrorProvider();
            toolTip = new ToolTip();
        }

        public static void AppendMsg(string msg) {
            if (TextBox_realtime_log != null && !TextBox_realtime_log.IsDisposed) {
                if (TextBox_realtime_log.InvokeRequired) {
                    TextBox_realtime_log.BeginInvoke(() =>
                        TextBox_realtime_log.AppendText($"{msg.Replace("\0", "")}\r\n")
                    );
                } else {
                    TextBox_realtime_log?.AppendText($"{msg.Replace("\0", "")}\r\n");
                }
            }
        }

        public static void SetError(Control ctrl, string message) {
            // Set error to control
            errorProvider.SetIconPadding(ctrl, 3);
            errorProvider.SetError(ctrl, message);

            // Show pop up message
            if (!string.IsNullOrEmpty(message)) {
                toolTip.Show(message, ctrl, 0, 30, 3000);
            }
        }

        public static bool ShowConfirmPopUp(string message)
            => MessageBox.Show(null, message, "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        public static DialogResult ShowNoticePopUp(string message)
            => MessageBox.Show(null, message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static DialogResult ShowWarningPopUp(string message)
            => MessageBox.Show(null, message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static DialogResult ShowErrorPopUp(string message)
            => MessageBox.Show(null, message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

    }
}
