using System.Collections.Generic;
using M.YZ.Basement.Core.Contracts.Data.Queries;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.QueryModels;

namespace M.YZ.Basement.MiniBlog.Core.Domain.Writers.Repositories
{
    public interface IWriterQueryRepository : IQueryRepository
    {
        public PagedData<List<WriterSummary>> Select(IWriterByFirstName writerByFirstName);
    }
}
