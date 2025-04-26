using Newtonsoft.Json;

namespace STaTool.utils {
    public static class FileUtil {
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

        /// <summary>
        /// Checks if the specified file is currently locked by another process.
        /// </summary>
        /// <param name="filePath">The full path to the file.</param>
        /// <returns>True if the file is locked; otherwise, false.</returns>
        public static bool IsFileLocked(string filePath) {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath)) return false;

            try {
                // Attempt to open the file in exclusive mode
                using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

                // If successful, the file is not locked
                return false;
            } catch (IOException) {
                // If an IOException occurs, the file is likely locked by another process
                return true;
            } catch (Exception ex) {
                // Log any unexpected exceptions
                Console.WriteLine($"Unexpected error while checking file lock: {ex}");
                throw;
            }
        }

        public static string Year() => DateTime.Now.ToString("yyyy");

        public static string Month() => DateTime.Now.ToString("MM");

        public static string Day() => DateTime.Now.ToString("dd");

        public static void CheckAndCreateFolder(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }
    }
}