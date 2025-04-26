namespace STaTool.constants {
    public static class FileType {
        public const string XLSX = ".xlsx";
        public const string CSV = ".csv";
        public const string TXT = ".txt";

        public static List<string> GetAll() {
            return new() { XLSX, CSV, TXT };
        }
    }
}