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

            // Begin by clearing all database contents.
            foreach (var currency in context.Currency)
            {
                context.Currency.Remove(currency);
            }
            foreach (var user in context.Users)
            {
                context.Users.Remove(user);
            }

            context.SaveChanges();

            await context.Currency.AddRangeAsync(currencies);
            await context.Users.AddRangeAsync(users);

            await context.SaveChangesAsync();
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
                new User() { Id = 1, FirstName = "Zachary", LastName = "Cauchi", Email = "foo@fizzbuzz.com" },
                new User() { Id = 2, FirstName = "Someone", LastName = "", Email = "bar@fizzbazz.com" }
            };
        }
    }
}
