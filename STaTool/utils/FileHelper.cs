using System.Reflection;
using StaTool.attribute;

namespace STaTool.utils {
    public static class FileHelper {
        public static string? CurrentPath { get; set; }

        public static string GetFileName() {
            return $"{FileUtil.Year()}-{FileUtil.Month()}-{FileUtil.Day()}";
        }

        public static string GetFileName(string fileType) {
            return $"{GetFileName()}{fileType}";
        }

        public static List<string> GetHeader(Type type) {
            ArgumentNullException.ThrowIfNull(type);

            return type.GetProperties()
                        .Where(property => property.GetCustomAttribute<FieldName>() != null)
                        .OrderBy(property => property.GetCustomAttribute<FieldName>()?.Order ?? 0)
                        .Select(property => property.GetCustomAttribute<FieldName>()?.Name ?? property.Name)
                        .ToList();
        }
    }
}