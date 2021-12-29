using M.YZ.Basement.Core.Domain.Entities;
using M.YZ.Basement.MiniBlog.Core.Domain.People.Events;

namespace M.YZ.Basement.MiniBlog.Core.Domain.People.Entities
{
    public class Person : AggregateRoot
    {
        private Person()
        {
        }

        public Person(string firstName, string lastName, int? age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            AddEvent(new PersonCreated
            {
                FirstName = FirstName,
                LastName = LastName,
                PersonId = BusinessId.Value.ToString()
            });
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
    }
}
