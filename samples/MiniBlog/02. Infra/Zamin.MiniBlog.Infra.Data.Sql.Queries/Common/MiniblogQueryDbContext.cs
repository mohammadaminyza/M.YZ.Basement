using M.YZ.Basement.Infra.Data.Sql.Queries;
using Microsoft.EntityFrameworkCore;

namespace M.YZ.Basement.MiniBlog.Infra.Data.Sql.Queries.Common
{
    public class MiniblogQueryDbContext : BaseQueryDbContext
    {
        public MiniblogQueryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
