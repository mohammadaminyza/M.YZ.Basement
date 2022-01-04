using System.Linq.Expressions;

namespace M.YZ.Basement.Infra.Data.MongoDb.Configuration;

public class EntityTypeBuilder<TEntity>
{
    public MongoPropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression)
    {
        return new MongoPropertyBuilder<TProperty>();
    }
}