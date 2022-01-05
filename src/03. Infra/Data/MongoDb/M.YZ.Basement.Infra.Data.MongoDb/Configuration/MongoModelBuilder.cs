using System.Linq.Expressions;

namespace M.YZ.Basement.Infra.Data.MongoDb.Configuration;

public class MongoModelBuilder<TProperty>
{
    public MongoPropertyBuilder<TProperty> HasConversion<TProvider>(
        Expression<Func<TProperty, TProvider>> convertToProviderExpression,
        Expression<Func<TProvider, TProperty>> convertFromProviderExpression)
    {
        return null;
    }
    public MongoPropertyBuilder<TProperty> SetBsonId<TProvider>(
        Expression<Func<TProperty, TProvider>> convertToProviderExpression,
        Expression<Func<TProvider, TProperty>> convertFromProviderExpression)
    {
        return null;
    }
}