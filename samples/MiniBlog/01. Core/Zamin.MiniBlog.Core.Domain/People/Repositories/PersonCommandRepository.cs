using M.YZ.Basement.Core.Contracts.Data.Commands;
using M.YZ.Basement.MiniBlog.Core.Domain.People.Entities;

namespace M.YZ.Basement.MiniBlog.Core.Domain.People.Repositories
{
    public interface IPersonCommandRepository : ICommandRepository<Person>
    {
    }
}
