using Newtonsoft.Json;

namespace STaTool.utils {
    public static class ConfigFileUtil {
        private const string CONFIG_FILE_PATH = "config.json";

        public static void SaveConfig(Config config) {
            string configJson = JsonConvert.SerializeObject(config);
            File.WriteAllText(CONFIG_FILE_PATH, configJson);
        }

        public static Config LoadConfig() {
            if (!File.Exists(CONFIG_FILE_PATH)) {
                return new Config();
            }

            string configJson = File.ReadAllText(CONFIG_FILE_PATH);
            return JsonConvert.DeserializeObject<Config>(configJson) ?? new Config();
        }

        public static bool IsPathValid(string path) {
            bool isValid;
            try {
                if (string.IsNullOrWhiteSpace(path)) {
                    isValid = false;
                }

                Path.GetFullPath(path);
                string? root = Path.GetPathRoot(path);
                isValid = string.IsNullOrEmpty(root?.Trim(new char[] { '\\', '/' })) == false;

                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
            } catch (Exception) {
                isValid = false;
            }

            return isValid;
        }
    }
}