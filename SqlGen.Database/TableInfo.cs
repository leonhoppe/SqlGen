namespace SqlGen.Database;

public struct TableInfo {
    public string Name { get; set; }
    public IList<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();
    
    public TableInfo() {}
}

public struct AttributeInfo {
    public string Name { get; init; }
    public string Type { get; init; }

    public string GetMappedName() {
        switch (Type) {
            case "varchar":
            case "text":
            case "longtext":
                return "String";
            
            case "int":
                return "Int32";
            
            case "tinyint":
                return "Int8";
            
            case "bigint":
                return "Int64";
            
            case "float":
                return "Single";
            
            case "timestamp":
            case "datetime":
                return "Date";
            
            default:
                return "Default";
        }
    }
}