namespace SqlGen.Config.Templates;

public sealed class DownloadConfig : IConfig {
    public const string DefaultName = "templates.json";
    
    public string[] Templates { get; set; }
}