using M.YZ.Basement.Infra.Data.Sql.Commands;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Entities;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Repositories;
using M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.Common;
using Zamin.MiniBlog.Infra.Data.Sql.Commands.Common;

namespace M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.Writers
{
    public class WriterCommandRepository : BaseCommandRepository<Writer, MiniblogDbContext>, IWriterRepository
    {
        public WriterCommandRepository(MiniblogDbContext dbContext) : base(dbContext)
        {
        }
    }
}
