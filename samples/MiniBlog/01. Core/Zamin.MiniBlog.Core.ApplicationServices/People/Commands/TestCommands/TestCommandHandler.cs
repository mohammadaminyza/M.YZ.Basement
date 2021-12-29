using M.YZ.Basement.Core.ApplicationServices.Commands;
using M.YZ.Basement.MiniBlog.Core.Domain.People.Repositories;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Repositories;
using M.YZ.Basement.Utilities;
using System.Threading.Tasks;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.People.Commands.TestCommands
{
    public class TestCommandHandler : CommandHandler<TestCommand>
    {
        public TestCommandHandler(BasementServices basementServices, IPersonCommandRepository personCommandRepository, IWriterQueryRepository writerQueryRepository) : base(basementServices)
        {
        }

        public override Task<CommandResult> Handle(TestCommand request)
        {
            return OkAsync();
        }
    }
}
