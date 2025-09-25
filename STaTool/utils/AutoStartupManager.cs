using Microsoft.Win32;
using log4net;

namespace STaTool.utils {
    /// <summary>
    /// 开机自启动管理类
    /// </summary>
    public static class AutoStartupManager {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoStartupManager));
        private const string REGISTRY_KEY_PATH = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string APP_NAME = "STaTool";

        /// <summary>
        /// 设置开机自启动
        /// </summary>
        /// <param name="enable">是否启用自启动</param>
        /// <returns>操作是否成功</returns>
        public static bool SetAutoStartup(bool enable) {
            try {
                RegistryKey? key = Registry.LocalMachine.OpenSubKey(REGISTRY_KEY_PATH, true);
                key ??= Registry.LocalMachine.CreateSubKey(REGISTRY_KEY_PATH, true);

                if (enable) {
                    string appPath = Application.ExecutablePath;
                    key.SetValue(APP_NAME, $"\"{appPath}\"");
                    log.Info($"已设置开机自启动: {appPath}");
                } else {
                    key.DeleteValue(APP_NAME, false);
                    log.Info("已取消开机自启动");
                }

                key.Close();
                return true;
            } catch (Exception ex) {
                log.Error($"设置开机自启动失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 检查是否已设置开机自启动
        /// </summary>
        /// <returns>是否已设置自启动</returns>
        // NOTE: Based on the provided context, this method does not appear to be called anywhere in the application.
        // It could be used, for example, in the MainForm's initialization (e.g., constructor or Load event)
        // to set the initial state of the `checkBox_auto_startup` based on the actual registry setting.
        public static bool IsAutoStartupEnabled() {
            try {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY_PATH, false)) {
                    if (key == null) return false;
                    return key.GetValue(APP_NAME) != null;
                }
            } catch (Exception ex) {
                log.Error($"检查开机自启动状态失败: {ex.Message}", ex);
                return false;
            }
        }
    }
}
