namespace SqlGen.Config.Projects;

public sealed class ProjectConfig : IConfig {
    public const string DefaultName = "sqlgen.config.json";
    
    public string TemplatePath { get; set; }
    public string BoilerplateFolder { get; set; }
    public ProjectDatabaseConfig Database { get; set; }
}