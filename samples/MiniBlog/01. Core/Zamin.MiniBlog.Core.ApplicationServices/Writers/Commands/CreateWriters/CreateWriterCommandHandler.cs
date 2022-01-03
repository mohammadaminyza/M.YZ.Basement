using System.Threading.Tasks;
using M.YZ.Basement.Core.ApplicationServices.Commands;
using M.YZ.Basement.Core.Contracts.ApplicationServices.Commands;
using M.YZ.Basement.Core.Domain.ValueObjects;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Entities;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Repositories;
using M.YZ.Basement.Utilities;

namespace M.YZ.Basement.MiniBlog.Core.ApplicationServices.Writers.Commands.CreateWriters
{
    public class CreateWriterCommandHandler : CommandHandler<CreateWiterCommand, long>
    {
        private readonly IWriterRepository _writerRepository;

        public CreateWriterCommandHandler(BasementServices basementServices, IWriterRepository writerRepository) : base(basementServices)
        {
            _writerRepository = writerRepository;
        }

        public override Task<CommandResult<long>> Handle(CreateWiterCommand request)
        {

            Writer writer = new Writer(BusinessId.FromGuid(request.BusinessId), request.FirstName, request.LastName);
            _writerRepository.Insert(writer);
            _writerRepository.Commit();
            return OkAsync(writer.Id);
        }
    }
}
