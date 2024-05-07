namespace SqlGen.Config.Templates;

public sealed class TypeMappings {
    public string String { get; set; } = "string";
    public string Char { get; set; } = "char";
    public string Int8 { get; set; } = "byte";
    public string Int16 { get; set; } = "short";
    public string Int32 { get; set; } = "int";
    public string Int64 { get; set; } = "long";
    public string Single { get; set; } = "float";
    public string Double { get; set; } = "double";
    public string Date { get; set; } = "DateTime";
    public string Default { get; set; } = "object";

    public string this[string type] {
        get {
            return (string)GetType().GetProperties().SingleOrDefault(p => p.Name == type)?.GetValue(this);
        }
    }
}