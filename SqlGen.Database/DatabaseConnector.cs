using SqlGen.Config.Projects;

namespace SqlGen.Database;

internal abstract class DatabaseConnector<TConfig> : IDatabaseConnector where TConfig : ProjectDatabaseConfig {
    public abstract void LoadConfig(TConfig config);

    public void LoadConfig(object config) {
        LoadConfig((TConfig) config);
    }

    public abstract Task<bool> Connect();

    public abstract Task Disconnect();

    public abstract Task<IList<TableInfo>> ExtractSchema();
}

internal interface IDatabaseConnector {
    public void LoadConfig(object config);
    public Task<bool> Connect();
    public Task Disconnect();
    public Task<IList<TableInfo>> ExtractSchema();
}
