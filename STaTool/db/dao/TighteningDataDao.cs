using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SQLite;
using System.Reflection;
using Dapper;
using Dapper.Contrib.Extensions;
using STaTool.db.extensions;
using STaTool.db.models;

namespace STaTool.db.dao {
    public class TighteningDataDao {
        private readonly SQLiteConnection _dbConnection;
        private string TableName { get; set; } = "tightening_data";

        public TighteningDataDao() {
            _dbConnection = DbUtils.GetConnection();

            var tighteningData = new TighteningData();
            string? tableName = tighteningData.GetTableName();
            if (tableName != null) {
                TableName = tableName;

                bool isTableExists = _dbConnection.TableExists(tableName);
                if (!isTableExists) {
                    _dbConnection.Execute(GetCreateTableSql());
                }
            }
        }
        private string GetCreateTableSql() {
            var properties = typeof(TighteningData).GetProperties();

            var columns = properties.Select(p =>
                $"{GetColumnName(p)} {GetSqlType(p)} {GetConstraints(p)}");

            // Bring 'id' to the front
            columns = columns.OrderBy(item => item.Contains("PRIMARY KEY") ? 0 : 1);

            return $"CREATE TABLE IF NOT EXISTS {TableName} ({string.Join(", ", columns)});";
        }
        private static string GetColumnName(PropertyInfo prop) {
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            return columnAttr?.Name ?? prop.Name;
        }

        private static string GetSqlType(PropertyInfo prop) {
            var type = prop.PropertyType;

            // Handle (Nullable<T>)
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                type = Nullable.GetUnderlyingType(type);
            }

            return Type.GetTypeCode(type) switch {
                TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 => "INTEGER",
                TypeCode.String => "TEXT",
                TypeCode.Boolean => "INTEGER", // SQLite use INTEGER for boolean
                TypeCode.DateTime => "TEXT",
                TypeCode.Decimal or TypeCode.Double or TypeCode.Single => "REAL",
                TypeCode.Byte or TypeCode.SByte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64 => "INTEGER",
                _ => type == typeof(Guid) ? "TEXT" : "TEXT" // GUID is treated as TEXT
            };
        }

        private static string GetConstraints(PropertyInfo prop) {
            if (prop.GetCustomAttribute<KeyAttribute>() != null)
                return "PRIMARY KEY AUTOINCREMENT";
            if (prop.GetCustomAttribute<ExplicitKeyAttribute>() != null)
                return "PRIMARY KEY";
            return "";
        }

        public List<TighteningData> GetAll() {
            return _dbConnection.GetAll<TighteningData>().ToList();
        }

        public int Insert(TighteningData data) {
            return (int) _dbConnection.Insert(data);
        }

        public bool Update(TighteningData data) {
            return _dbConnection.Update<TighteningData>(data);
        }

        public bool Delete(TighteningData data) {
            return _dbConnection.Delete(data);
        }
    }
}