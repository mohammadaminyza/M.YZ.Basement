using System.Threading.Tasks;
using M.YZ.Basement.Core.ApplicationServices.Commands;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;
using M.YZ.Basement.MiniBlog.Core.Domain.People.Entities;
using M.YZ.Basement.MiniBlog.Core.Domain.People.Repositories;
using M.YZ.Basement.Utilities;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.People.Commands.CreatePerson
{
    public class CreatePersonCommandHandler : CommandHandler<CreatePersonCommand, long>
    {
        private readonly IPersonCommandRepository _personRepository;

        public CreatePersonCommandHandler(BasementServices basementServices, IPersonCommandRepository personRepository) : base(basementServices)
        {
            _personRepository = personRepository;

        }

        public override  Task<CommandResult<long>> Handle(CreatePersonCommand request)
        {
           // throw new InvalidEntityStateException("test");
            Person person = new Person(request.FirstName, request.LastName, null);
            _personRepository.Insert(person);
             _personRepository.Commit();
            return OkAsync(person.Id);
        }
    }
}
