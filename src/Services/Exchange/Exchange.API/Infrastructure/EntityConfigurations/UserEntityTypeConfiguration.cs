using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exchange.API.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration
        : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .UseHiLo("user_hilo")
                .IsRequired();

            builder.Property(c => c.FirstName)
                .IsRequired(false)
                .HasMaxLength(128);

            builder.Property(c => c.LastName)
                .IsRequired(false)
                .HasMaxLength(128);

            builder.Property(c => c.Email)
                .IsRequired(true)
                .HasMaxLength(128);

            builder.HasMany(u => u.AccountBalances)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .IsRequired();
        }
    }
}
