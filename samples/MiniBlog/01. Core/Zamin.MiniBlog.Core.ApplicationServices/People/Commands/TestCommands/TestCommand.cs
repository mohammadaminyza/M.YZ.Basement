using M.YZ.Basement.Core.ApplicationServices.Commands;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.People.Commands.TestCommands
{
    public class TestCommand : ICommand
    {
        public string Name { get; set; }
    }
}
