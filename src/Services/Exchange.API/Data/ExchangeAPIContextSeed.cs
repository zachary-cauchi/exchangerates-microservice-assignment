using Exchange.API.Models;

namespace Exchange.API.Data
{
    public class ExchangeAPIContextSeed
    {

        public async Task ClearAndSeedAsync(ExchangeAPIContext context)
        {
            var currencies = GetCurrencyData();

            // Begin by clearing all database contents.
            foreach (var currency in context.Currency)
            {
                context.Currency.Remove(currency);
            }
            context.SaveChanges();

            await context.Currency.AddRangeAsync(currencies);

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
    }
}
