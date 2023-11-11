using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.UnitTests
{
    internal class DataBuilders
    {
        public static Currency[] GetValidCurrencyPair()
        {
            return new Currency[]
            {
                new Currency() { Id = 1, Name = "Euro", Code = "EUR" },
                new Currency() { Id = 2, Name = "Dollar", Code = "USD" }
            };
        }

        public static ExchangeRate GetValidExchangeRate()
        {
            return new ExchangeRate("EUR", "USD", DateTime.UtcNow, "Test", 2);
        }

        public static AccountBalance[] GetValidAccountBalancePair()
        {
            return new AccountBalance[]
            {
                new AccountBalance() { Id = 1, UserId = 1, CurrencyId = 1, Balance = 100 },
                new AccountBalance() { Id = 2, UserId = 1, CurrencyId = 2, Balance = 100 }
            };
        }

        public static User GetValidUser()
        {
            return new User(id: 1, email: "test@foo.com", firstName: "Foo", lastName: "Bar");
        }

        public static PastTransaction GetValidPastTransaction()
        {
            return new PastTransaction()
            {
                Id = 1,
                UserId = 1,
                FromAccountBalanceId = 1,
                ToAccountBalanceId = 2,
                DebitedAmount = 20,
                CreditedAmount = 40,
                TimeEffected = DateTime.UtcNow,
                FromCurrencyId = 1,
                ToCurrencyId = 2,
                ExchangeRate = 2,
            };
        }

        public static void MockCacheKeyForReturnValue(Mock<IDistributedCache> cache, string key, string value)
        {
            // Extension methods aren't supported, so we have to mock the internal non-extended function.
            cache.Setup(x => x.GetAsync(It.Is<string>(y => y == key), It.IsAny<CancellationToken>())).ReturnsAsync(() => Encoding.UTF8.GetBytes(value));
        }
    }
}
