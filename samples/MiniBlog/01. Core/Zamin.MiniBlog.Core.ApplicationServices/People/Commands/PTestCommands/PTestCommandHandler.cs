using System.Threading.Tasks;
using M.YZ.Basement.Core.ApplicationServices.Commands;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;
using M.YZ.Basement.Utilities;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.People.Commands.PTestCommands
{
    public class PTestCommandHandler : CommandHandler<PTestCommand>
    {
        public PTestCommandHandler(BasementServices basementServices) : base(basementServices)
        {
        }

        public override Task<CommandResult> Handle(PTestCommand request)
        {
            return OkAsync();
        }
    }
}
