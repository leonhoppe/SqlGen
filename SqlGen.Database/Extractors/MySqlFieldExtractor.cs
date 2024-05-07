using System.Data;
using MySqlConnector;
using SqlGen.Config.Projects.Database;

namespace SqlGen.Database.Extractors;

internal sealed class MySqlFieldExtractor : DatabaseConnector<MySqlConfig> {
    private MySqlConfig Config { get; set; }
    private MySqlConnection Connection { get; set; }

    public override void LoadConfig(MySqlConfig config) {
        Config = config;
    }

    public override async Task<bool> Connect() {
        var builder = new MySqlConnectionStringBuilder();
        builder.Server = Config.Address;
        builder.Port = Convert.ToUInt32(Config.Port);
        builder.UserID = Config.Username;
        builder.Password = Config.Password;
        builder.Database = Config.Database;
        
        Connection = new MySqlConnection(builder.ToString());

        try {
            await Connection.OpenAsync();
        } catch (Exception e) {
            await Console.Error.WriteLineAsync($"[{e.Source}] {e.Message}");
            return false;
        }

        return Connection.State == ConnectionState.Open;
    }

    public override async Task Disconnect() {
        try {
            await Connection?.CloseAsync()!;
        } catch (Exception e) {
            await Console.Error.WriteLineAsync($"[{e.Source}] {e.Message}");
        }
    }

    public override async Task<IList<TableInfo>> ExtractSchema() {
        var tables = new List<TableInfo>();

        var data = await Connection.GetSchemaAsync("Columns");
        TableInfo currentTable = new TableInfo();
        foreach (DataRow entry in data.Rows) {
            if ((string)entry[1] != Config.Database) continue;
            
            var table = (string)entry[2];
            if (Config.ExcludeTables?.Contains(table) == true) continue;
            
            var column = (string)entry[3];
            var type = (string)entry[7];

            if (string.IsNullOrEmpty(currentTable.Name)) {
                currentTable.Name = table;
            } if (currentTable.Name != table) {
                tables.Add(currentTable);
                currentTable = new TableInfo();
                currentTable.Name = table;
            }
            
            currentTable.Attributes.Add(new AttributeInfo {
                Name = column,
                Type = type
            });
        }
        tables.Add(currentTable);

        return tables;
    }
}