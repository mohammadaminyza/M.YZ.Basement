namespace M.YZ.Basement.Utilities.Configurations;
public class AppPartOptions
{
    public bool Enabled { get; set; } = true;
    public bool AutoCreateSqlTable { get; set; } = true;
    public string ApplicationTableName { get; set; } = "Applications";
    public string ControllerTableName { get; set; } = "Controllers";
    public string ActionTableName { get; set; } = "Actions";
    public string SchemaName { get; set; } = "dbo";
    public string ConnectionString { get; set; }
}
