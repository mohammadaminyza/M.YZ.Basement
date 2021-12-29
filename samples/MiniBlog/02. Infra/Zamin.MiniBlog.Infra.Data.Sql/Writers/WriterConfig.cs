using M.YZ.Basement.Core.Domain.Toolkits.ValueObjects;
using M.YZ.Basement.MiniBlog.Core.Domain.Writers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace M.YZ.Basement.MiniBlog.Infra.Data.Sql.Commands.Writers
{
    public class WriterConfig : IEntityTypeConfiguration<Writer>
    {
        public void Configure(EntityTypeBuilder<Writer> builder)
        {
            builder.Property(c => c.FirstName).HasConversion(c => c.Value, c => Title.FromString(c));
            builder.Property(c => c.LastName).HasConversion(c => c.Value, c => Title.FromString(c));
        }
    }
}
