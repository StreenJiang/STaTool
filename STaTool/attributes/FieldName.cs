namespace StaTool.attribute {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldName: Attribute {
        public string Name { get; }
        public int Order { get; }
        public Type? EnumType { get; }

        public FieldName(string name, int order = 0, Type? enumType = null) {
            Name = name;
            Order = order;
            EnumType = enumType;
        }
    }
}