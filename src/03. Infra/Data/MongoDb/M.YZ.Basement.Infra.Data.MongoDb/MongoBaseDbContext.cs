using M.YZ.Basement.Infra.Data.MongoDb.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace M.YZ.Basement.Infra.Data.MongoDb;

public abstract class MongoBaseDbContext
{
    private readonly MongoUrl _connectionString;
    private Dictionary<TrackerType, List<TrackedModel<dynamic>>> _insertTrackerModels = new();
    private Dictionary<TrackerType, List<TrackedModel<dynamic>>> _updateTrackerModels = new();
    private Dictionary<TrackerType, List<TrackedModel<dynamic>>> _deleteTrackerModels = new();

    protected IMongoClient MongoClient { get; set; }
    protected IMongoDatabase MongoDatabase { get; set; }

    public string GetMongoCollectionNameByEntityDefaultName<TEntity>() =>
        GetCollectionName<TEntity>();
    public IMongoCollection<TEntity> GetMongoCollection<TEntity>() =>
        MongoDatabase.GetCollection<TEntity>(GetCollectionName<TEntity>());
    public IMongoCollection<TEntity> GetMongoCollection<TEntity>(string collectionName) =>
        MongoDatabase.GetCollection<TEntity>(collectionName);

    private string GetCollectionName<TEntity>()
    {
        return this.GetType().GetProperties()?.FirstOrDefault(p => p.PropertyType == typeof(ICollection<TEntity>) || p.PropertyType == typeof(IMongoCollection<TEntity>))?.Name;
    }

    protected string DatabaseName => _connectionString.DatabaseName;

    protected MongoBaseDbContext(IMongoClient mongoClient, MongoDbContextOption dbContextOption)
    {
        _connectionString = MongoUrl.Create(dbContextOption.ConnectionString);
        MongoClient = mongoClient;
        MongoDatabase = MongoClient.GetDatabase(_connectionString.DatabaseName);
    }

    private IClientSessionHandle _clientSessionHandle;

    #region Add
    public virtual void Add<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var trackerType = new TrackerType(nameof(entity), typeof(TEntity));

        InsertTrackerManager(trackerType, entity);

        //if (_insertTrackerModels.TryGetValue(trackerType, out var existingModel) && existingModel.Any(p => p.State == TrackedModelState.Added))
        //{
        //    _insertTrackerModels[trackerType] = existingModel[_insertTrackerModels.Keys.].WithNewState(TrackedModelState.Added);
        //    return;
        //}
    }

    public virtual Task AddAsync<TEntity>(TEntity entity) where TEntity : class => Task.Run(() => { Add(entity); });

    public virtual void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        if (entities == null || !entities.Any())
        {
            throw new ArgumentNullException(nameof(entities));
        }

        var trackerType = new TrackerType(nameof(TEntity), typeof(TEntity));

        foreach (var entity in entities)
        {
            InsertTrackerManager(trackerType, entity);
        }

        //if (_insertTrackerModels.TryGetValue(trackerType, out var existingModel) && existingModel.State == TrackedModelState.Added)
        //{
        //    _insertTrackerModels[trackerType] = existingModel.WithNewState(TrackedModelState.Added);
        //    return;
        //}
    }

    #endregion

    #region Update

    public void Update<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var trackerType = new TrackerType(nameof(entity), typeof(TEntity));

    }

    public Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class => Task.Run(() => { Update(entity); });

    #endregion

    #region Remove

    public virtual void Remove<TEntity>(TEntity entity) where TEntity : class
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        var trackerType = new TrackerType(nameof(entity), typeof(TEntity));

        RemoveTrackerManager(trackerType, entity);
    }

    public virtual Task RemoveAsync<TEntity>(TEntity entity) where TEntity : class => Task.Run(() => { Remove(entity); });

    #endregion

    #region Any

    public bool Any<TEntity>(Expression<Func<TEntity, bool>> expression)
    {
        var collection = GetMongoCollection<TEntity>();
        return collection.FindSync(expression).Any();
    }

    public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression)
    {
        var collection = GetMongoCollection<TEntity>();
        return await (await collection.FindAsync(expression)).AnyAsync();
    }

    #endregion

    public virtual void SaveChanges()
    {
        //TODO Active Transaction

        try
        {
            //StartClientSection();

            ExecuteCommitHandler();

            // await CommitTransactionAsync();

            EmptyTacker();
        }
        catch (Exception e)
        {
            //await RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public virtual async Task SaveChangesAsync()
    {
        //TODO Active Transaction

        try
        {
            //await StartClientSectionAsync();
            //await Task.Run(BeginTransaction);
            using var session = await MongoClient.StartSessionAsync();

            //var transactionOptions = new TransactionOptions(
            //    readPreference: ReadPreference.Primary,
            //    readConcern: ReadConcern.Local,
            //    writeConcern: WriteConcern.WMajority);

            //await session.WithTransactionAsync(async (s, ct) =>
            //{
            //}, transactionOptions);

            await Task.Run(ExecuteCommitHandler);

            await CommitTransactionAsync();

            await Task.Run(EmptyTacker);
        }
        catch (Exception e)
        {
            //await RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public virtual void BeginTransaction()
    {
        _clientSessionHandle.StartTransaction();
    }

    private void StartClientSection()
    {
        var clientSessionHandler = MongoClient.StartSession();
        _clientSessionHandle = clientSessionHandler;
    }

    private async Task StartClientSectionAsync()
    {
        var clientSessionHandler = await MongoClient.StartSessionAsync();
        _clientSessionHandle = clientSessionHandler;
    }

    public virtual void RollbackTransaction()
    {
        _clientSessionHandle.AbortTransaction();
    }

    public virtual async Task RollbackTransactionAsync()
    {
        await _clientSessionHandle.AbortTransactionAsync();
    }

    public virtual void CommitTransaction()
    {
        _clientSessionHandle.CommitTransaction();
    }

    public virtual async Task CommitTransactionAsync()
    {
        await _clientSessionHandle.CommitTransactionAsync();
    }

    private void ExecuteCommitHandler()
    {
        TrackedModelCommitHandler commitHandler = new TrackedModelCommitHandler(MongoClient, MongoDatabase);

        foreach (var trackerModel in _insertTrackerModels)
        {
            var collectionName = GetCollectionByTrackerModelEntityType(trackerModel);

            if (trackerModel.Value.Count() == 1)
            {
                commitHandler.HandleCommitManager(trackerModel.Value.Single(), MongoDatabase, collectionName);
            }
            else
            {
                commitHandler.HandleCommitManager(trackerModel.Value, MongoDatabase, collectionName);
            }
        }
    }

    private void EmptyTacker()
    {
        _insertTrackerModels = new();
        _updateTrackerModels = new();
        _deleteTrackerModels = new();
    }

    private void InsertTrackerManager<TEntity>(TrackerType trackerType, TEntity entity)
    {
        if (_insertTrackerModels.Any(p => p.Key == trackerType))
        {
            if (_insertTrackerModels.Any(p => p.Key == trackerType && !p.Value.Any(v => v.Model == entity)))
            {

                var currentDic = _insertTrackerModels.Where(p => p.Key == trackerType).Select(p => p.Value)
                    .SingleOrDefault();
                currentDic.Add(TrackedModel<dynamic>.Add(entity));

                _insertTrackerModels.Remove(trackerType);
                _insertTrackerModels.Add(trackerType, currentDic);
            }

            return;
        }
        else
        {
            _insertTrackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { TrackedModel<dynamic>.Add(entity) });
        }
    }
    private void UpdateTrackerManager<TEntity>(TrackerType trackerType, TEntity entity)
    {
        if (_updateTrackerModels.Any(p => p.Key == trackerType))
        {
            if (_updateTrackerModels.Any(p => p.Key == trackerType && !p.Value.Any(v => v.Model == entity)))
            {

                var currentDic = _updateTrackerModels.Where(p => p.Key == trackerType).Select(p => p.Value)
                    .SingleOrDefault();

                currentDic.Add(new(entity, TrackedModelState.Update));

                _updateTrackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { TrackedModel<dynamic>.Update(entity) });
            }

            return;
        }
        else
        {
            _updateTrackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { TrackedModel<dynamic>.Update(entity) });
        }
    }
    private void RemoveTrackerManager<TEntity>(TrackerType trackerType, TEntity entity)
    {
        if (_deleteTrackerModels.Any(p => p.Key == trackerType))
        {
            if (_deleteTrackerModels.Any(p => p.Key == trackerType))
            {
                if (_deleteTrackerModels.Any(p => p.Key == trackerType && !p.Value.Any(v => v.Model == entity)))
                {
                    var currentTracker = _deleteTrackerModels.Values.ToList().Single();
                    currentTracker.Add(new TrackedModel<dynamic>(entity, TrackedModelState.Deleted));


                    _deleteTrackerModels.Remove(trackerType);
                    _deleteTrackerModels.Add(trackerType, currentTracker);
                }

                return;
            }

            _deleteTrackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { new(entity, TrackedModelState.Deleted) });
        }
        else
        {
            _deleteTrackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { new(entity, TrackedModelState.Deleted) });
        }
    }

    private string GetCollectionByTrackerModelEntityType(KeyValuePair<TrackerType, List<TrackedModel<dynamic>>> trackerModel)
    {
        var trackerType = trackerModel.Key.Type;

        MethodInfo getCollectionType = GetType().GetMethod("GetMongoCollectionNameByEntityDefaultName")
            ?.MakeGenericMethod(trackerType);

        var collection = getCollectionType?.Invoke(this, null).ToString();

        return collection;
    }
}