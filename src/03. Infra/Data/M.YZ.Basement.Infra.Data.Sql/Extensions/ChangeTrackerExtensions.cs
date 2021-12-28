using Microsoft.EntityFrameworkCore.ChangeTracking;
using M.YZ.Basement.Core.Domain.Entities;

namespace M.YZ.Basement.Infra.Data.Sql.Extensions;
public static class ChangeTrackerExtensions
{
    public static List<AggregateRoot> GetChangedAggregates(this ChangeTracker changeTracker) =>
   changeTracker.Entries<AggregateRoot>()
                            .Where(x => x.State == EntityState.Modified ||
                                   x.State == EntityState.Added ||
                                   x.State == EntityState.Deleted).Select(c => c.Entity).ToList();

    public static List<AggregateRoot> GetAggregatesWithEvent(this ChangeTracker changeTracker) =>
            changeTracker.Entries<AggregateRoot>()
                                     .Where(x => x.State != EntityState.Detached).Select(c => c.Entity).Where(c => c.GetEvents().Any()).ToList();

}
