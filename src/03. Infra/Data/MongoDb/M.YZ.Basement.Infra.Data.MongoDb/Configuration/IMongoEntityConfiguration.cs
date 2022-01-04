namespace M.YZ.Basement.Infra.Data.MongoDb.Configuration;

public interface IMongoEntityConfiguration<TEntity>
{
    void Configuration(MongoModelBuilder<TEntity> builder);
}