using System.Text.Json.Serialization;

namespace Exchange.API.Models
{
    public class PastTransaction : IAggregateRoot
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        //[JsonIgnore]
        public User User { get; set; } = null!;

        public int FromAccountBalanceId { get; set; }

        //[JsonIgnore]
        public AccountBalance FromAccountBalance { get; set; } = null!;

        public int ToAccountBalanceId { get; set; }

        //[JsonIgnore]
        public AccountBalance ToAccountBalance { get; set; } = null!;

        public decimal DebitedAmount { get; set; }

        public decimal CreditedAmount => DebitedAmount * ExchangeRate;

        public DateTime TimeEffected { get; set; }

        public int FromCurrencyId { get; set; }

        //[JsonIgnore]
        public Currency FromCurrency { get; set; } = null!;

        public int ToCurrencyId { get; set;}

        //[JsonIgnore]
        public Currency ToCurrency { get; set; } = null!;

        public decimal ExchangeRate { get; set; }
    }
}
