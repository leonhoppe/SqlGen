// See https://aka.ms/new-console-template for more information

using SqlGen.Cli;
using SqlGen.Cli.Subroutines;
using SqlGen.Cli.Utils;

ConsoleManager.PrintMotd();

var operations = ConsoleManager.ExtractCliOptions(args);

if (operations.Count == 0) {
    operations.Add(await ConsoleManager.PromptOperation());
}

foreach (var (operation, argument) in operations) {
    switch (operation) {
        case CliOption.ShowHelp:
            ConsoleManager.ShowHelpText();
            break;
        
        case CliOption.DownloadTemplates:
            await new TemplateDownloader().Execute(argument);
            break;
        
        case CliOption.GenerateTemplateConfig:
            await new TemplateGenerator().Execute(argument);
            break;
        
        case CliOption.CreateProjectConfig:
            await new ConfigCreator().Execute(argument);
            break;
        
        case CliOption.RunProjectConfig:
            await new ConfigExecutor().Execute(argument);
            break;
    }
}
