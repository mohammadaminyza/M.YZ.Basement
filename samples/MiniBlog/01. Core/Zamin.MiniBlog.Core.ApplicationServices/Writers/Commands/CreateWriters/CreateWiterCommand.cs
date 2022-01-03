using System;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.Writers.Commands.CreateWriters
{
    public class CreateWiterCommand : ICommand<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid BusinessId { get; set; }
    }
}
