using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.API.Infrastructure.EntityConfigurations
{
    public class PastTransactionEntityTypeConfiguration
        : IEntityTypeConfiguration<PastTransaction>
    {
        public void Configure(EntityTypeBuilder<PastTransaction> builder)
        {
            builder.ToTable("past_transaction");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .UseHiLo("past_transaction_hilo")
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(p => p.FromAccountBalance)
                .WithMany()
                .HasForeignKey(p => p.FromAccountBalanceId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(p => p.ToAccountBalance)
                .WithMany()
                .HasForeignKey(p => p.ToAccountBalanceId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(p => p.FromCurrency)
                .WithMany()
                .HasForeignKey(p => p.FromCurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(p => p.ToCurrency)
                .WithMany()
                .HasForeignKey(p => p.ToCurrencyId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Property(p => p.DebitedAmount)
                .HasColumnType("decimal(19,4)")
                .IsRequired();

            builder.Property(p => p.CreditedAmount)
                .HasColumnType("decimal(19,4)")
                .IsRequired();

            builder.Property(p => p.TimeEffected)
                .IsRequired();

            builder.Property(p => p.ExchangeRate)
                .HasColumnType("decimal(19,4)")
                .IsRequired();
        }
    }
}
