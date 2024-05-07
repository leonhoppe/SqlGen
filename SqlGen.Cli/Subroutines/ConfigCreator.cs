using SqlGen.Cli.Utils.Menu;
using SqlGen.Config;
using SqlGen.Config.Projects;
using SqlGen.Config.Projects.Database;

namespace SqlGen.Cli.Subroutines;

public sealed class ConfigCreator : ISubroutine {
    public async Task Execute(string cmdArg) {
        var prompt = new Prompt<string>();
        
        var templatesDir = await prompt.Present("Templates folder", TemplateDownloader.DownloadDestination);
        templatesDir = Path.GetFullPath(templatesDir);

        var templates = new DirectoryInfo(templatesDir).EnumerateDirectories().Select(dir => dir.Name).ToArray();

        Console.WriteLine("\nSelect Template:");
        var select = new Multiselect();
        var template = await select.Present(templates);

        var config = new ProjectConfig();
        config.TemplatePath = templatesDir + Path.DirectorySeparatorChar + templates[template];
        config.BoilerplateFolder = await prompt.Present("Boilerplate folder", "src/records");

        Console.WriteLine("\nSelect Database type:");
        var databaseTypes = new[] { "mysql" }; //TODO: dynamically fetch supported db types
        switch (databaseTypes[await select.Present(databaseTypes)]) {
            case "mysql":
                config.Database = await SetupMySql();
                break;
        }

        var loader = new Loader();
        loader.Present("Generating config...");

        await ConfigLoader<ProjectConfig>.SaveConfig(config, Path.GetFullPath(cmdArg));

        await loader.Destroy();
    }

    private async Task<MySqlConfig> SetupMySql() {
        var prompt = new Prompt<string>();
        
        var config = new MySqlConfig();
        config.Address = await prompt.Present("Address", "localhost");
        config.Port = await new Prompt<ushort>().Present("Port", 3306);
        config.Username = await prompt.Present("Username");
        config.Password = await prompt.Present("Password");
        config.Database = await prompt.Present("Database");

        return config;
    }
}