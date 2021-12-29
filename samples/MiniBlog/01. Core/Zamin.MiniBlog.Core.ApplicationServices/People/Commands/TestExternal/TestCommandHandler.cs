using System.Threading.Tasks;
using M.YZ.Basement.Core.ApplicationServices.Commands;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;
using M.YZ.Basement.Utilities;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.People.Commands.TestExternal
{
    public class TestCommandHandler : CommandHandler<TestCommand>
    {
        public TestCommandHandler(BasementServices basementServices) : base(basementServices)
        {
        }

        public override Task<CommandResult> Handle(TestCommand request)
        {
            int a = 123;
            return OkAsync();
        }
    }
}
