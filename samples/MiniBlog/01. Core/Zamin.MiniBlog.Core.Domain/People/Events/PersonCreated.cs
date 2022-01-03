using M.YZ.Basement.Core.Domain.Events;

namespace M.YZ.Basement.MiniBlog.Core.Domain.People.Events
{
    public class PersonCreated : IDomainEvent
    {
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class PersonUpdated : IDomainEvent
    {
        public string PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
