using System.Collections.Generic;
using System.Threading.Tasks;
using M.YZ.Basement.Infra.Data.ChangeInterceptors.EntityChageInterceptorItems;
using M.YZ.Basement.Infra.Data.MongoDb;
using M.YZ.Basement.Utilities.Configurations;
using MongoDB.Driver;

namespace M.YZ.Basement.Infra.Data.ChangeInterceptors.MongoDb;

public class MongoEntityChangeInterceptorItemRepository : IEntityChageInterceptorItemRepository
{
    private readonly MongoBaseDbContext _mongoBaseDbContext;
    private readonly BasementConfigurationOptions _configuration;
    private string CollectionName => _configuration.EntityChangeInterception
        .MongoEntityChangeInterceptorItemRepositoryOptions.EntityChangeInterceptorItemCollectionName;

    public MongoEntityChangeInterceptorItemRepository(BasementConfigurationOptions configurationOptions)
    {
        _configuration = configurationOptions;
        _mongoBaseDbContext = new MongoEntityChangeInterceptorItemContext(new MongoClient(), new MongoDbContextOption(configurationOptions.EntityChangeInterception.MongoEntityChangeInterceptorItemRepositoryOptions.ConnectionString));
    }

    public void Save(List<EntityChageInterceptorItem> entityChangeInterceptorItems)
    {
        var collection = _mongoBaseDbContext.GetMongoCollection<EntityChageInterceptorItem>(CollectionName);
        collection.InsertMany(entityChangeInterceptorItems);
    }

    public async Task SaveAsync(List<EntityChageInterceptorItem> entityChangeInterceptorItems)
    {
        var collection = _mongoBaseDbContext.GetMongoCollection<EntityChageInterceptorItem>(CollectionName);
        await collection.InsertManyAsync(entityChangeInterceptorItems);
    }
}