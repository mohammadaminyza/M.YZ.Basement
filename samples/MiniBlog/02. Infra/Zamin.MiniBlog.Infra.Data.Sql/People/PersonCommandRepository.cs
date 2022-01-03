using M.YZ.Basement.Infra.Data.Sql.Commands;
using M.YZ.Basement.MiniBlog.Core.Domain.People.Entities;
using M.YZ.Basement.MiniBlog.Core.Domain.People.Repositories;
using M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.Common;

namespace M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.People
{
    public class PersonCommandRepository : BaseCommandRepository<Person, MiniblogDbContext>, IPersonCommandRepository
    {
        public PersonCommandRepository(MiniblogDbContext dbContext) : base(dbContext)
        {
        }
    }
}
