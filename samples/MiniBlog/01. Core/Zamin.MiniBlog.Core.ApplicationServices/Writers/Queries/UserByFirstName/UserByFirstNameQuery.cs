using System.Collections.Generic;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Queries;
using M.YZ.Basement.Core.Contracts.Data.Queries;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.QueryModels;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.Writers.Queries.UserByFirstName
{
    public class UserByFirstNameQuery : PageQuery<PagedData<List<WriterSummary>>>, IWriterByFirstName
    {
        public string FirstName { get; set; }
    }
}
