using M.YZ.Basement.Infra.Data.MongoDb;

namespace M.YZ.Basement.Infra.Data.ChangeInterceptors.MongoDb;

public class MongoEntityChangeInterceptorItemContext : MongoBaseDbContext
{
    public MongoEntityChangeInterceptorItemContext(IMongoClient mongoClient, MongoDbContextOption dbContextOption) : base(mongoClient, dbContextOption)
    {
    }
}