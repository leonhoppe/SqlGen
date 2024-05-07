namespace SqlGen.Config.Projects.Database;

public sealed class MySqlConfig : ProjectDatabaseConfig {
    public string Address { get; set; }
    public ushort Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }
}