using Exchange.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Exchange.API.Data
{
    public class ExchangeAPIContextSeed
    {

        public async Task ClearAndSeedAsync(ExchangeAPIContext context)
        {
            var currencies = GetCurrencyData();
            var users = GetUserData();
            var accountBalances = GetAccountBalanceData();
            var pastTransactions = GetPastTransactionsData();

            // Begin by clearing all database contents.
            PurgeDbSet(context.PastTransactions);
            PurgeDbSet(context.AccountBalances);
            PurgeDbSet(context.Currency);
            PurgeDbSet(context.Users);

            context.SaveChanges();

            await context.Currency.AddRangeAsync(currencies);
            await context.Users.AddRangeAsync(users);
            await context.AccountBalances.AddRangeAsync(accountBalances);
            await context.PastTransactions.AddRangeAsync(pastTransactions);

            await context.SaveChangesAsync();
        }

        private void PurgeDbSet<T>(DbSet<T> dbSet) where T : class
        {
            foreach (var entry in dbSet)
            {
                dbSet.Remove(entry);
            }
        }

        private IEnumerable<Currency> GetCurrencyData()
        {
            return new List<Currency>()
            {
                new Currency() { Id = 1, Name = "Euro", Code = "EUR" },
                new Currency() { Id = 2, Name = "Maltese Liri", Code = "MTL" },
                new Currency() { Id = 3, Name = "United States Dollar", Code = "USD" }
            };
        }

        private IEnumerable<User> GetUserData()
        {
            return new List<User>()
            {
                new User(id: 1, firstName: "Zachary", lastName: "Cauchi", email: "foo@fizzbuzz.com"),
                new User(id: 2, firstName: "Someone", lastName: "", email: "bar@fizzbazz.com")
            };
        }

        private IEnumerable<AccountBalance> GetAccountBalanceData()
        {
            return new List<AccountBalance>()
            {
                new AccountBalance() { Id = 1, UserId = 1, CurrencyId = 1, Balance = 1337 },
                new AccountBalance() { Id = 2, UserId = 1, CurrencyId = 2, Balance = 333 },
                new AccountBalance() { Id = 3, UserId = 1, CurrencyId = 3, Balance = 999 }
            };
        }

        public IEnumerable<PastTransaction> GetPastTransactionsData()
        {
            return new List<PastTransaction>()
            {
                new PastTransaction() { Id = 1, UserId = 1, FromAccountBalanceId = 2, ToAccountBalanceId = 1, DebitedAmount = 20, TimeEffected = new DateTime(2023, 11, 08, 06, 06, 06), ExchangeRate = 2, FromCurrencyId = 2, ToCurrencyId = 1 }
            };
        }
    }
}
