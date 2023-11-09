namespace Exchange.API.Models
{
    public class PastTransaction : IAggregateRoot
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;

        public int FromAccountBalanceId { get; set; }

        public AccountBalance FromAccountBalance { get; set; } = null!;

        public int ToAccountBalanceId { get; set; }

        public AccountBalance ToAccountBalance { get; set; } = null!;

        public decimal DebitedAmount { get; set; }

        public decimal CreditedAmount => DebitedAmount * ExchangeRate;

        public DateTime TimeEffected { get; set; }

        public int FromCurrencyId { get; set; }

        public Currency FromCurrency { get; set; } = null!;

        public int ToCurrencyId { get; set;}

        public Currency ToCurrency { get; set; } = null!;

        public decimal ExchangeRate { get; set; }

        public PastTransaction()
        {

        }

        public PastTransaction(User user, AccountBalance fromAccountBalance, AccountBalance toAccountBalance, decimal debitedAmount, DateTime timeEffected, Currency fromCurrency, Currency toCurrency, decimal exchangeRate)
        {
            UserId = user.Id;
            FromAccountBalanceId = fromAccountBalance.Id;
            FromCurrencyId = fromCurrency.Id;
            ToCurrencyId = toCurrency.Id;
            ToAccountBalanceId = toAccountBalance.Id;
            DebitedAmount = debitedAmount;
            TimeEffected = timeEffected;
            FromCurrencyId = fromCurrency.Id;
            ToCurrencyId = toCurrency.Id;
            ExchangeRate = exchangeRate;
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
        }
    }
}
