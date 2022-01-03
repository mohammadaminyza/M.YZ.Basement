using M.YZ.Basement.Core.Contracts.Data.Commands;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Entities;

namespace M.YZ.Basement.MiniBlog.Core.Domain.Writers.Repositories
{
    public interface IWriterRepository : ICommandRepository<Writer>
    {
    }
}
