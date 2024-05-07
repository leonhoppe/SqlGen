using Newtonsoft.Json;

namespace SqlGen.Config;

public static class ConfigLoader<T> where T : IConfig {
    
    public static async Task<T> LoadConfig(string path) {
        if (!File.Exists(path)) return default;

        var raw = await File.ReadAllTextAsync(path);
        return LoadConfigFromRaw(raw);
    }

    public static async Task SaveConfig(T config, string path) {
        var json = JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        await File.WriteAllTextAsync(path, json);
    }

    public static T LoadConfigFromRaw(string content) {
        return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
    }
    
}