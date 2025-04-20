using System.Reflection;
using Dapper.Contrib.Extensions;
using STaTool.utils;

namespace STaTool.db.models {
    public abstract class AEntityBase {
        [Key]
        public int id { get; set; }
        public int user_id { get; set; } = -1;
        public int deleted { get; set; } = (int) YesOrNo.NO;
        public string creator { get; set; } = "system";
        public string modifier { get; set; } = "system";
        public DateTime create_time { get; set; } = DateTime.Now;
        public DateTime modify_time { get; set; } = DateTime.Now;

        public string? GetTableName() {
            var tableAttr = GetType().GetCustomAttribute<TableAttribute>();
            return tableAttr?.Name ?? null;
        }
    }
}