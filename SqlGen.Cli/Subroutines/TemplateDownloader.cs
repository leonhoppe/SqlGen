using SqlGen.Cli.Utils.Menu;
using SqlGen.Config;
using SqlGen.Config.Templates;

namespace SqlGen.Cli.Subroutines;

public sealed class TemplateDownloader : ISubroutine {
    public static readonly string DownloadDestination = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "SqlGen" + Path.DirectorySeparatorChar + "Templates";
    
    public async Task Execute(string cmdArg) {
        var spinner = new Loader();
        spinner.Present("Downloading manifest...");

        using var client = new HttpClient();
        var manifest = await DownloadFile<DownloadConfig>(client, cmdArg);

        await spinner.Destroy();
        
        var multiselect = new Multiselect();
        var options = manifest.Templates.ToList();
        options.Insert(0, "All");
        Console.WriteLine("\nSelect Template to download:");
        var templateIndex = await multiselect.Present(options.ToArray());

        Console.WriteLine();
        var dest = await new Prompt<string>().Present("Templates folder", DownloadDestination);
        Console.WriteLine();

        spinner = new Loader();
        spinner.Present("Downloading template(s)...");

        var baseUrl = new Uri(new Uri(cmdArg), ".").OriginalString;
        if (templateIndex == 0) {
            foreach (var template in manifest.Templates) {
                await DownloadTemplate(baseUrl, template, dest, client);
            }
        }
        else {
            await DownloadTemplate(baseUrl, manifest.Templates[templateIndex - 1], dest, client);
        }

        await spinner.Destroy();
    }
    
    private async Task<T> DownloadFile<T>(HttpClient client, string url) where T : IConfig {
        var content = await client.GetStringAsync(url);
        return ConfigLoader<T>.LoadConfigFromRaw(content);
    }

    private async Task DownloadTemplate(string baseUrl, string name, string dest, HttpClient client) {
        baseUrl += Uri.EscapeDataString(name + '/');
        dest = dest + Path.DirectorySeparatorChar + name + Path.DirectorySeparatorChar;

        Directory.CreateDirectory(dest);
        
        var manifest = await DownloadFile<TemplateConfig>(client, baseUrl + "template.json");
        await ConfigLoader<TemplateConfig>.SaveConfig(manifest, dest + "template.json");

        var outline = await client.GetStringAsync(baseUrl + manifest.OutlineTemplate);
        await File.WriteAllTextAsync(dest + manifest.OutlineTemplate, outline);

        var record = await client.GetStringAsync(baseUrl + manifest.RecordTemplate);
        await File.WriteAllTextAsync(dest + manifest.RecordTemplate, record);
    }
}