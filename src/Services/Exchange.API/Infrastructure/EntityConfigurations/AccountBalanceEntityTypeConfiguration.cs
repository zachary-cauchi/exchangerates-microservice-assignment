using Exchange.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Exchange.API.Infrastructure.EntityConfigurations
{
    public class AccountBalanceEntityTypeConfiguration
        : IEntityTypeConfiguration<AccountBalance>
    {
        public void Configure(EntityTypeBuilder<AccountBalance> builder)
        {
            builder.ToTable("account_balance");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .UseHiLo("account_balance_hilo")
                .IsRequired();

            builder.HasOne(a => a.User)
                .WithMany(u => u.AccountBalances)
                .HasForeignKey(a => a.UserId)
                .IsRequired();

            builder.HasOne(a => a.Currency)
                .WithMany()
                .HasForeignKey(a => a.CurrencyId)
                .IsRequired();

            builder.Property(a => a.Balance)
                .HasColumnType("decimal(19,4)")
                .IsRequired();
        }
    }
}
