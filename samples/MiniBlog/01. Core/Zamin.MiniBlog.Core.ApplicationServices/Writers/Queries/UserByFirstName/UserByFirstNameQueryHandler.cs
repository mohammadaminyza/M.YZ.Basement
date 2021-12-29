using System.Collections.Generic;
using System.Threading.Tasks;
using M.YZ.Basement.Core.ApplicationServices.Queries;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Queries;
using M.YZ.Basement.Core.Contracts.Data.Queries;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.QueryModels;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Repositories;
using M.YZ.Basement.Utilities;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.Writers.Queries.UserByFirstName
{
    public class UserByFirstNameQueryHandler : QueryHandler<UserByFirstNameQuery, PagedData<List<WriterSummary>>>
    {
        private readonly IWriterQueryRepository repository;

        public UserByFirstNameQueryHandler(BasementServices basementApplicationContext, IWriterQueryRepository repository) : base(basementApplicationContext)
        {
            this.repository = repository;
        }

        public override Task<QueryResult<PagedData<List<WriterSummary>>>> Handle(UserByFirstNameQuery request)
        {
            var result = repository.Select(request);
            return ResultAsync(result);

        }
    }
}
