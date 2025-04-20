using System.Data.SQLite;
using Dapper;

namespace STaTool.db.extensions {
    public static class DapperExtensions {
        public static bool TableExists(this SQLiteConnection connection, string tableName) {
            // SQLite check table exists
            var sql = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name=@tableName";
            return connection.ExecuteScalar<int>(sql, new { tableName }) > 0;
        }
    }
}