using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Exchange.API.Models;
using Microsoft.EntityFrameworkCore.Design;
using Exchange.API.Infrastructure.EntityConfigurations;

namespace Exchange.API.Data
{
    public class ExchangeAPIContext : DbContext, IUnitOfWork
    {
        public ExchangeAPIContext (DbContextOptions<ExchangeAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Currency> Currency { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<AccountBalance> AccountBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CurrencyEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
            builder.ApplyConfiguration(new AccountBalanceEntityTypeConfiguration());
        }
    }
}
