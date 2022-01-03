using M.YZ.Basement.Core.Contracts.ApplicationServices.Queries;

namespace M.YZ.Basement.MiniBlog.Core.Domain.Writers.QueryModels
{
    public interface IWriterByFirstName : IPageQuery<WriterSummary>
    {
        public string FirstName { get; set; }
    }
}
