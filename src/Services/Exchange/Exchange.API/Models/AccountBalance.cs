
namespace Exchange.API.Models
{
    public class AccountBalance : IAggregateRoot
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;

        public int CurrencyId { get; set; }

        [JsonIgnore]
        public Currency Currency { get; set; } = null!;

        public decimal Balance { get; set; }
    }
}
