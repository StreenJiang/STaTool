namespace STaTool.utils {
    public static class ArgumentValidator {
        public static void ValidateInt(int arguement, string errorMsg) {
            if (arguement <= 0) {
                throw new ArgumentNullException(errorMsg);
            }
        }
        
        public static bool ValidateIPv4(object ipString) {
            if (ipString != null && ipString is string) {
                return ValidateIPv4(ipString.ToString());
            }
            return false;
        }

        public static bool ValidateIPv4(string ipString) {
            if (string.IsNullOrWhiteSpace(ipString)) return false;

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4) return false;

            bool result = splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
            if (result) {
                // Firt byte can't be less than 1
                for (int i = 0; i < splitValues.Length; i++) {
                    bool canParse = int.TryParse(splitValues[i], out int value);
                    if (!canParse) {
                        return false;
                    }
                    if (i == 0 && (value < 1 || value > 255)) {
                        return false;
                    } else if (value < 0 || value > 255) {
                        return false;
                    }
                }
            }
            return result;
        }

        public static bool ValidatePortInWindows(object portString) {
            if (portString != null && (portString is string || portString is int)) {
                return ValidatePortInWindows(portString.ToString());
            }
            return false;
        }

        public static bool ValidatePortInWindows(string portString) {
            if (string.IsNullOrWhiteSpace(portString)) return false;

            if (!int.TryParse(portString, out int port)) return false;

            return port >= 1 && port <= 65535;
        }
    }

}