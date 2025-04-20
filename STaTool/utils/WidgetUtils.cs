namespace STaTool.utils {
    public static class WidgetUtils {
        private static ErrorProvider errorProvider;
        private static ToolTip toolTip;

        static WidgetUtils() {
            errorProvider = new ErrorProvider();
            toolTip = new ToolTip();
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