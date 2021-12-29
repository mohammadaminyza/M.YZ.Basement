using M.YZ.Basement.Core.ApplicationServices.Commands;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.People.Commands.TestExternal
{
    public class TestCommand : ICommand
    {
        public string Name { get; set; }
    }
}
