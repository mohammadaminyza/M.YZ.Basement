namespace M.YZ.Basement.EndPoints.Web.MongoDb;

public static class MongoDbExtension
{
    public static IServiceCollection AddMongoDb<TContext>(this IServiceCollection services, Action<MongoDbContextOption> dbContextOptionAction) where TContext : MongoBaseDbContext
    {
        var dbContextOption = new MongoDbContextOption("");
        dbContextOptionAction(dbContextOption);

        services.AddScoped<IMongoClient>(c => new MongoClient(dbContextOption.ConnectionString));
        services.AddScoped<TContext>();
        services.AddSingleton<MongoDbContextOption>(p =>
            new MongoDbContextOption(dbContextOption.ConnectionString));

        return services;
    }
}