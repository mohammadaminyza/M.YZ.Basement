using System.Collections.Generic;
using M.YZ.Basement.Core.Contracts.Data.Queries;
using M.YZ.Basement.Infra.Data.Sql.Queries;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.QueryModels;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Repositories;
using M.YZ.Basement.MiniBlog.Infra.Data.Sql.Queries.Common;

namespace M.YZ.Basement.MiniBlog.Infra.Data.Sql.Queries.Writers
{
    public class SqlWriterQueryRepository : BaseQueryRepository<MiniblogQueryDbContext>, IWriterQueryRepository
    {
        public SqlWriterQueryRepository(MiniblogQueryDbContext dbContext) : base(dbContext)
        {
        }

        public PagedData<List<WriterSummary>> Select(IWriterByFirstName writerByFirstName)
        {
            return new PagedData<List<WriterSummary>>();
        }
    }
}
