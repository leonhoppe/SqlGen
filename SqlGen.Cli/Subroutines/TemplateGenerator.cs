using SqlGen.Cli.Utils.Menu;
using SqlGen.Config;
using SqlGen.Config.Templates;

namespace SqlGen.Cli.Subroutines;

public sealed class TemplateGenerator : ISubroutine {
    public async Task Execute(string cmdArg) {
        var prompt = new Prompt<string>();

        var path = await prompt.Present("Templates folder", TemplateDownloader.DownloadDestination);
        path = Path.GetFullPath(path);

        var config = new TemplateConfig();
        config.Name = await prompt.Present("Template name", "TypeScript");
        config.Language = await prompt.Present("Language file ending", "ts");
        config.OutlineTemplate = await prompt.Present("Outline template", "outline.ts");
        config.RecordTemplate = await prompt.Present("Record template", "record.ts");
        config.TypeMappings = new TypeMappings();

        var loader = new Loader();
        loader.Present("Generating template");

        var directory = path + Path.DirectorySeparatorChar + config.Name + Path.DirectorySeparatorChar;
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        
        await ConfigLoader<TemplateConfig>.SaveConfig(config, directory + TemplateConfig.DefaultName);
        await File.WriteAllTextAsync(directory + config.OutlineTemplate, null);
        await File.WriteAllTextAsync(directory + config.RecordTemplate, null);

        await loader.Destroy();
    }
}