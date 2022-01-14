using System.Linq.Expressions;

namespace M.YZ.Basement.Utilities.Services.ObjectMappers;
public interface IMapperAdapter
{
    TDestination Map<TSource, TDestination>(TSource source);

    IQueryable<TDestination> MapTo<TSource, TDestination>(IQueryable source,
        params Expression<Func<TDestination, object>>[] membersToExpand);
}
