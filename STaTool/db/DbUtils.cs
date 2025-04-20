using System.Data.SQLite;

namespace STaTool.db {
    public static class DbUtils {
        public static string DbPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "db");
        public static string DbName { get; } = "database.db";

        static DbUtils() {
            if (!Directory.Exists(DbPath)) {
                Directory.CreateDirectory(DbPath);
            }
        }

        public static SQLiteConnection GetConnection() {
            var dbPath = Path.Combine(DbPath, DbName);
            var connection = new SQLiteConnection($"Data Source={dbPath};Version=3;Connection Timeout=2;");
            connection.Open();
            return connection;
        }
    }
}