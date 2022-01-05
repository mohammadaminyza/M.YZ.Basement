using M.YZ.Basement.Infra.Data.MongoDb.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace M.YZ.Basement.Infra.Data.MongoDb;

public abstract class MongoBaseDbContext
{
    private readonly MongoUrl _connectionString;

    //Todo ReValuate Tracker And CommitHandler
    //Todo Change Tracker Files
    private Dictionary<TrackerType, List<TrackedModel<dynamic>>> _trackerModels = new();

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

        var trackerType = new TrackerType(nameof(entity), typeof(TEntity), TrackedModelState.Added);

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

        var trackerType = new TrackerType(nameof(TEntity), typeof(TEntity), TrackedModelState.Added);

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

        var trackerType = new TrackerType(nameof(entity), typeof(TEntity), TrackedModelState.Update);

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

        var trackerType = new TrackerType(nameof(entity), typeof(TEntity), TrackedModelState.Deleted);

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


    protected virtual void BeginTransaction()
    {
        _clientSessionHandle.StartTransaction();
    }

    /// <summary>
    /// Start MongoClient Session
    /// </summary>
    private void StartClientSection()
    {
        var clientSessionHandler = MongoClient.StartSession();
        _clientSessionHandle = clientSessionHandler;
    }

    protected async Task StartClientSectionAsync()
    {
        var clientSessionHandler = await MongoClient.StartSessionAsync();
        _clientSessionHandle = clientSessionHandler;
    }

    protected virtual void RollbackTransaction()
    {
        _clientSessionHandle.AbortTransaction();
    }

    protected virtual async Task RollbackTransactionAsync()
    {
        await _clientSessionHandle.AbortTransactionAsync();
    }

    protected virtual void CommitTransaction()
    {
        _clientSessionHandle.CommitTransaction();
    }

    protected virtual async Task CommitTransactionAsync()
    {
        await _clientSessionHandle.CommitTransactionAsync();
    }

    /// <summary>
    /// Execute Commit Handler For Commit Logic Manager
    /// </summary>
    private void ExecuteCommitHandler()
    {
        TrackedModelCommitHandler commitHandler = new TrackedModelCommitHandler(MongoClient, MongoDatabase);

        foreach (var trackerModel in _trackerModels)
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

    /// <summary>
    /// Rest The Tracker
    /// </summary>
    private void EmptyTacker()
    {
        _trackerModels = new();
    }

    private void InsertTrackerManager<TEntity>(TrackerType trackerType, TEntity entity)
    {
        if (_trackerModels.Any(p => p.Key == trackerType))
        {
            if (_trackerModels.Any(p => p.Key == trackerType && !p.Value.Any(v => v.Model == entity)))
            {

                var currentDic = _trackerModels.Where(p => p.Key == trackerType).Select(p => p.Value)
                    .SingleOrDefault();
                currentDic.Add(TrackedModel<dynamic>.Add(entity));

                _trackerModels.Remove(trackerType);
                _trackerModels.Add(trackerType, currentDic);
            }

            return;
        }
        else
        {
            _trackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { TrackedModel<dynamic>.Add(entity) });
        }
    }
    private void UpdateTrackerManager<TEntity>(TrackerType trackerType, TEntity entity)
    {
        if (_trackerModels.Any(p => p.Key == trackerType))
        {
            if (_trackerModels.Any(p => p.Key == trackerType && !p.Value.Any(v => v.Model == entity)))
            {
                var currentDic = _trackerModels.Where(p => p.Key == trackerType && p.Key.TrackerState == TrackedModelState.Update).Select(p => p.Value)
                    .SingleOrDefault();

                currentDic.Add(new(entity, TrackedModelState.Update));

                _trackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { TrackedModel<dynamic>.Update(entity) });
            }

            return;
        }
        else
        {
            _trackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { TrackedModel<dynamic>.Update(entity) });
        }
    }
    private void RemoveTrackerManager<TEntity>(TrackerType trackerType, TEntity entity)
    {
        if (_trackerModels.Any(p => p.Key == trackerType && !p.Value.Any(v => v.Model == entity)))
        {
            var currentTracker = _trackerModels.Values.ToList().Single();
            currentTracker.Add(new TrackedModel<dynamic>(entity, TrackedModelState.Deleted));


            _trackerModels.Remove(trackerType);
            _trackerModels.Add(trackerType, currentTracker);
        }
        else
        {
            _trackerModels.Add(trackerType, new List<TrackedModel<dynamic>>() { new(entity, TrackedModelState.Deleted) });
        }
    }

    private string GetCollectionByTrackerModelEntityType(KeyValuePair<TrackerType, List<TrackedModel<dynamic>>> trackerModel)
    {
        var trackerType = trackerModel.Key.Type;

        MethodInfo getCollectionType = GetType().GetMethod("GetMongoCollectionNameByEntityDefaultName")
            ?.MakeGenericMethod(trackerType);

        var collection = getCollectionType?.Invoke(this, null)?.ToString();

        if (string.IsNullOrEmpty(collection))
            throw new NullReferenceException("Collection Name Is Null Or Empty");

        return collection;
    }

    //public event EventHandler EntityChangedTracker;
}