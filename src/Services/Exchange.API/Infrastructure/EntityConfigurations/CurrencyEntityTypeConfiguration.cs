using Exchange.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.API.Infrastructure.EntityConfigurations
{
    public class CurrencyEntityTypeConfiguration
        : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("currency");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .UseHiLo("currency_hilo")
                .IsRequired();

            builder.Property(c => c.Name)
                .IsRequired(true)
                .HasMaxLength(128);

            builder.Property(c => c.Code)
                .IsRequired(true)
                .HasMaxLength(3);
        }
    }
}
