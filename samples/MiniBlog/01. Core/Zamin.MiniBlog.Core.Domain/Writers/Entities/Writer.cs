using M.YZ.Basement.Core.Domain.Entities;
using M.YZ.Basement.Core.Domain.Toolkits.ValueObjects;
using M.YZ.Basement.Core.Domain.ValueObjects;

namespace M.YZ.Basement.MiniBlog.Core.Domain.Writers.Entities
{
    public class Writer : AggregateRoot
    {
        public Title FirstName { get; private set; }
        public Title LastName { get; private set; }
        public Writer(BusinessId businessId, Title firstName, Title lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            BusinessId = businessId;
        }

        public void Update(Title firstName, Title lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
