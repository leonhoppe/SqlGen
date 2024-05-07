using SqlGen.Cli.Utils.Menu;
using SqlGen.Config;
using SqlGen.Config.Projects;
using SqlGen.Database;
using SqlGen.Generator;

namespace SqlGen.Cli.Subroutines;

public sealed class ConfigExecutor : ISubroutine {
    public async Task Execute(string cmdArg) {
        var loader = new Loader();
        loader.Present("Generating boilerplate...");
        
        var config = await ConfigLoader<ProjectConfig>.LoadConfig(cmdArg);

        var manager = new DatabaseManager();
        var tableData = await manager.ExecuteConfiguration(config.Database);

        await BoilerplateGenerator.GenerateBoilerplate(config, tableData);
        await loader.Destroy();
    }
}