namespace M.YZ.Basement.Utilities.Configurations;
public class EntityChangeInterceptionOptions
{
    public bool Enabled { get; set; } = false;
    public string EntityChageInterceptorRepositoryTypeName { get; set; } = "FakeEntityChageInterceptorItemRepository";
    public DapperEntityChageInterceptorItemRepositoryOptions DapperEntityChageInterceptorItemRepository { get; set; }
        = new DapperEntityChageInterceptorItemRepositoryOptions();
    public MongoEntityChangeInterceptorItemRepositoryOptions MongoEntityChangeInterceptorItemRepositoryOptions { get; set; }
        = new MongoEntityChangeInterceptorItemRepositoryOptions();
}


public class DapperEntityChageInterceptorItemRepositoryOptions
{

    public string ConnectionString { get; set; } = string.Empty;
    public bool AutoCreateSqlTable { get; set; } = true;
    public string EntityChageInterceptorItemTableName { get; set; } = "EntityChageInterceptorItem";
    public string EntityChageInterceptorItemSchemaName { get; set; } = "dbo";
    public string PropertyChangeLogItemTableName { get; set; } = "ParrotTranslations";
    public string PropertyChangeLogItemSchemaName { get; set; } = "PropertyChangeLogItem";
}

public class MongoEntityChangeInterceptorItemRepositoryOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string EntityChangeInterceptorItemCollectionName { get; set; } = "EntityChangeInterceptorItem";
    public string EntityChangeInterceptorItemSchemaName { get; set; } = "dbo";
    public string PropertyChangeLogItemCollectionName { get; set; } = "ParrotTranslations";
    public string PropertyChangeLogItemSchemaName { get; set; } = "PropertyChangeLogItem";
}