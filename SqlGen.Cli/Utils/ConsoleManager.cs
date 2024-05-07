using SqlGen.Cli.Utils.Menu;
using SqlGen.Config.Projects;
using SqlGen.Config.Templates;

namespace SqlGen.Cli.Utils;

internal static class ConsoleManager {
    private const string AssetFolder = "./Assets/";
    private const string DefaultTemplateCdn = "https://cdn.leon-hoppe.de/sqlgen/" + DownloadConfig.DefaultName;

    public static void PrintMotd() {
        var motd = File.ReadAllText(AssetFolder + "motd.txt");
        Console.WriteLine(motd);
    }

    public static IList<Tuple<CliOption, string>> ExtractCliOptions(string[] args) {
        var options = new List<Tuple<CliOption, string>>();

        /*if (args.Length == 0) {
            options.Add(new Tuple<CliOption, string>(CliOption.RunProjectConfig, ProjectConfig.DefaultName));
            return options;
        }*/
        
        for (int i = 0; i < args.Length; i++) {
            var arg = args[i];

            if (arg == "-d" || arg == "--download") {
                var uri = DefaultTemplateCdn;
                
                if (i < args.Length - 1 && !args[i + 1].StartsWith("-")) {
                    uri = args[i + 1];
                    i++;
                }
                
                options.Add(new Tuple<CliOption, string>(CliOption.DownloadTemplates, uri));
            }
            
            else if (arg == "-g" || arg == "--generate") {
                options.Add(new Tuple<CliOption, string>(CliOption.GenerateTemplateConfig, null));
            }

            else if (arg == "-c" || arg == "--create") {
                var configPath = ProjectConfig.DefaultName;
                
                if (i < args.Length - 1 && !args[i + 1].StartsWith("-")) {
                    configPath = args[i + 1];
                    i++;
                }
                
                options.Add(new Tuple<CliOption, string>(CliOption.CreateProjectConfig, configPath));
            }

            else if (arg == "-r" || arg == "--run") {
                var configPath = ProjectConfig.DefaultName;
                
                if (i < args.Length - 1 && !args[i + 1].StartsWith("-")) {
                    configPath = args[i + 1];
                    i++;
                }
                
                options.Add(new Tuple<CliOption, string>(CliOption.RunProjectConfig, configPath));
            }
            
            else if (arg == "-h" || arg == "--help" || arg == "/help") {
                options.Add(new Tuple<CliOption, string>(CliOption.ShowHelp, null));
            }
        }

        return options;
    }

    public static async Task<Tuple<CliOption, string>> PromptOperation() {
        Console.WriteLine("Select operation:");
        var select = new Multiselect();
        var option = await select.Present(new[] {
            "Download Templates", 
            "Generate Tenplate Config",
            "Create Project Configuration", 
            "Run Project Configuration", 
            "Show Help"
        });
        Console.WriteLine();

        if (option == 4) return new Tuple<CliOption, string>(CliOption.ShowHelp, null);
        
        var propt = new Prompt<string>();

        if (option == 0) {
            var uri = await propt.Present("Repository Url", DefaultTemplateCdn);
            Console.WriteLine();
            return new Tuple<CliOption, string>(CliOption.DownloadTemplates, uri);
        }

        if (option == 1) return new Tuple<CliOption, string>(CliOption.GenerateTemplateConfig, null);

        if (option == 2 || option == 3) {
            var path = await propt.Present("Config file", ProjectConfig.DefaultName);
            Console.WriteLine();
            return new Tuple<CliOption, string>(option == 2 ? CliOption.CreateProjectConfig : CliOption.RunProjectConfig, path);
        }

        return new Tuple<CliOption, string>(CliOption.ShowHelp, null);
    }

    public static void ShowHelpText() {
        var helpText = File.ReadAllText(AssetFolder + "help.txt");
        Console.WriteLine(helpText);
    }
    
}