using SqlGen.Config.Projects;
using SqlGen.Config.Projects.Database;
using SqlGen.Database.Extractors;

namespace SqlGen.Database;

public sealed class DatabaseManager {
    private IDictionary<Type, Type> Extractors { get; }

    public DatabaseManager() {
        Extractors = new Dictionary<Type, Type>();
        
        AddExtractor<MySqlFieldExtractor, MySqlConfig>();
    }

    private void AddExtractor<THandler, TConfig>() where THandler : IDatabaseConnector where TConfig : ProjectDatabaseConfig{
        Extractors.Add(typeof(TConfig), typeof(THandler));
    }

    public async Task<IList<TableInfo>> ExecuteConfiguration(ProjectDatabaseConfig config) {
        if (config is MySqlConfig mysql) {
            var handlerType = Extractors[typeof(MySqlConfig)];
            var handlerInstance = Activator.CreateInstance(handlerType) as IDatabaseConnector;
            if (handlerInstance == null)
                throw new NullReferenceException("Handler for " + handlerType.Name + " not found.");
            
            handlerInstance.LoadConfig(config);
            await handlerInstance.Connect();
            var data = await handlerInstance.ExtractSchema();
            await handlerInstance.Disconnect();

            return data;
        }

        return null;
    }
}