using LM.MSEmail.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LM.MSEmail.Api.Infra.Mapping
{
    public class HistoryMapping : IEntityTypeConfiguration<History>
    {
        public void Configure(EntityTypeBuilder<History> builder)
        {
            builder.ToTable(nameof(History));

            builder.OwnsOne(p => p.To, to =>
            {
                to.Property(p => p.Name).HasColumnName("To_Name");
                to.Property(x => x.Email).HasColumnName("To_Email");
            });

            builder.OwnsOne(p => p.From, from =>
            {
                from.Property(p => p.Name).HasColumnName("From_Name");
                from.Property(x => x.Email).HasColumnName("From_Email");
            });
        }
    }
}
