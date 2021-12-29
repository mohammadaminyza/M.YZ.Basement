using System;
using System.Threading.Tasks;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Events;
using M.YZ.Basement.MiniBlog.Core.Domain.People.Events;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.Writers.Events
{
    public class PersonCreatedHandler : IDomainEventHandler<PersonCreated>
    {
        public Task Handle(PersonCreated Event)
        {
            Console.WriteLine(Event.FirstName);
            return Task.CompletedTask;
        }
    }

    public class PersonUpdatedHandler : IDomainEventHandler<PersonUpdated>
    {
        public Task Handle(PersonUpdated Event)
        {
            Console.WriteLine(Event.FirstName);
            return Task.CompletedTask;
        }
    }
}
