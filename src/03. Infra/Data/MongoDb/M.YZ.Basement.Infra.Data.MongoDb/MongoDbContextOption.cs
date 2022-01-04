namespace M.YZ.Basement.Infra.Data.MongoDb;

public class MongoDbContextOption
{
    public string ConnectionString { get; set; }

    public MongoDbContextOption(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public MongoDbContextOption()
    {

    }
}