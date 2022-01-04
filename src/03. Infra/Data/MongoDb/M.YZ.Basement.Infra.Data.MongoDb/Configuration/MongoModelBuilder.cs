using System.Linq.Expressions;

namespace M.YZ.Basement.Infra.Data.MongoDb.Configuration;

public class MongoModelBuilder<TProperty>
{
    //public MongoProperty Property<TEntity>(Expression<Func<TEntity, TProperty>> selection)
    //{
    //    var compilede = selection.Compile();

    //    return null;
    //}

    public MongoPropertyBuilder<TProperty> HasConversion<TProvider>(
        Expression<Func<TProperty, TProvider>> convertToProviderExpression,
        Expression<Func<TProvider, TProperty>> convertFromProviderExpression)
    {
        return null;
    }
}