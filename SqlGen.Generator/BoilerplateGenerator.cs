using SqlGen.Config;
using SqlGen.Config.Projects;
using SqlGen.Config.Templates;
using SqlGen.Database;

namespace SqlGen.Generator;

public static class BoilerplateGenerator {
    public static async Task GenerateBoilerplate(ProjectConfig config, IList<TableInfo> records) {
        var templateConfig = await ConfigLoader<TemplateConfig>.LoadConfig(config.TemplatePath + Path.DirectorySeparatorChar + TemplateConfig.DefaultName);

        var outlineTemplate = await File.ReadAllTextAsync(config.TemplatePath + Path.DirectorySeparatorChar + templateConfig.OutlineTemplate);
        var recordTemplate = await File.ReadAllTextAsync(config.TemplatePath + Path.DirectorySeparatorChar + templateConfig.RecordTemplate);
        
        foreach (var record in records) {
            var code = GenerateRecord(record, outlineTemplate, recordTemplate, templateConfig.TypeMappings);

            if (!Directory.Exists(config.BoilerplateFolder))
                Directory.CreateDirectory(config.BoilerplateFolder);

            await File.WriteAllTextAsync(config.BoilerplateFolder + Path.DirectorySeparatorChar + record.Name + "." + templateConfig.Language, code);
        }
    }

    private static string GenerateRecord(TableInfo info, string outline, string record, TypeMappings mappings) {
        var records = new List<string>();

        foreach (var attribute in info.Attributes) {
            var type = attribute.GetMappedName();
            var result = record.Replace("%record_name%", attribute.Name).Replace("%record_type%", mappings[type]);
            records.Add(result);
        }

        return outline.Replace("%record_name%", info.Name).Replace("%records%", string.Join('\n', records));
    }
}