namespace M.YZ.Basement.Infra.Data.MongoDb.ChangeTracking;

internal class TrackedModelCommitHandler
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;

    public void HandleCommitManager<TEntity>(TrackedModel<TEntity> trackerModel, IMongoDatabase mongoDatabase, string collectionName)
    {
        var collectionBson = GetMongoBsonCollection(mongoDatabase, collectionName);

        switch (trackerModel.State)
        {
            case TrackedModelState.Added:
                {
                    Insert(trackerModel.Model, collectionBson);
                    break;
                }
            case TrackedModelState.Update:
                {
                    //Todo Complete
                    break;
                }
        }
    }
    public void HandleCommitManager<TEntity>(List<TrackedModel<TEntity>> trackerModels, IMongoDatabase mongoDatabase, string collectionName)
    {
        var convertedTrackerModels = trackerModels as List<TrackedModel<TEntity>>;

        var collectionBson = GetMongoBsonCollection(mongoDatabase, collectionName);

        var models = new List<TEntity>();

        foreach (var trackerModel in convertedTrackerModels)
        {
            models.Add(trackerModel.Model);
        }

        switch (convertedTrackerModels.First().State)
        {
            case TrackedModelState.Added:
                {
                    InsertMany(models, collectionBson);
                    break;
                }
            case TrackedModelState.Update:
                {
                    //Todo Complete
                    break;
                }
        }
    }

    private void Insert<TEntity>(TEntity entity, IMongoCollection<BsonDocument> mongoCollection)
    {
        var bsonDocument = entity.ToBsonDocument();

        Insert(bsonDocument, mongoCollection);
    }

    private void Insert(BsonDocument bsonDocument, IMongoCollection<BsonDocument> mongoCollection)
    {
        mongoCollection.InsertOne(bsonDocument);
    }

    private void InsertMany<TEntity>(IEnumerable<TEntity> entities, IMongoCollection<BsonDocument> mongoCollection)
    {
        List<BsonDocument> bsonDocument = new List<BsonDocument>();

        foreach (var entity in entities)
        {
            bsonDocument.Add(entity.ToBsonDocument());
        }

        InsertMany(bsonDocument, mongoCollection);
    }
    private void InsertMany(IEnumerable<BsonDocument> bsonDocument, IMongoCollection<BsonDocument> mongoCollection)
    {
        mongoCollection.InsertMany(bsonDocument);
    }

    private async Task InsertAsync<TEntity>(TEntity entity, IMongoCollection<BsonDocument> mongoCollection)
    {
        var bsonDocument = entity.ToBsonDocument();

        await InsertAsync(bsonDocument, mongoCollection);
    }

    private async Task InsertAsync(BsonDocument bsonDocument, IMongoCollection<BsonDocument> mongoCollection)
    {
        await mongoCollection.InsertOneAsync(bsonDocument);
    }

    private IMongoCollection<BsonDocument> GetMongoBsonCollection(IMongoDatabase mongoDatabase, string collectionName)
    {
        var collectionBson = mongoDatabase.GetCollection<BsonDocument>(collectionName);

        return collectionBson;
    }

    public TrackedModelCommitHandler(IMongoClient mongoClient, IMongoDatabase mongoDatabase)
    {
        _mongoClient = mongoClient;
        _mongoDatabase = mongoDatabase;
    }
}