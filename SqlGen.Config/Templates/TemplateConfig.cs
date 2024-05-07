namespace SqlGen.Config.Templates;

public sealed class TemplateConfig : IConfig {
    public const string DefaultName = "template.json";
    
    public string Name { get; set; }
    public string Language { get; set; }

    public string OutlineTemplate { get; set; }
    public string RecordTemplate { get; set; }
    public TypeMappings TypeMappings { get; set; }
}
