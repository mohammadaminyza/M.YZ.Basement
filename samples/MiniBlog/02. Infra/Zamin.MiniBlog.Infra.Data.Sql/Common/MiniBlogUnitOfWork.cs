using M.YZ.Basement.Infra.Data.Sql.Commands;
using M.YZ.Basement.MiniBlog.Core.Domain;
using M.YZ.Basement.Utilities;
using Zamin.MiniBlog.Infra.Data.Sql.Commands.Common;

namespace M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.Common
{
    public class MiniBlogUnitOfWork : BaseEntityFrameworkUnitOfWork<MiniblogDbContext>, IMiniblogUnitOfWork
    {
        public MiniBlogUnitOfWork(MiniblogDbContext dbContext, BasementServices basementApplicationContext) : base(dbContext, basementApplicationContext)
        {
        }
    }
}
