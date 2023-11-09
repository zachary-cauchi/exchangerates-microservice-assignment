using System.Runtime.Serialization;

namespace Exchange.API.Commands
{
    public class CreateCurrencyExchangeCommand
        : IRequest<PastTransaction>
    {
        [DataMember]
        public int FromAccountBalanceId { get; private set; }

        [DataMember]
        public int ToAccountBalanceId { get; private set; }

        [DataMember]
        public decimal DebitAmount { get; private set; }

        [DataMember]
        public DateTime TimeOfRate { get; private set; }

        [DataMember]
        public decimal ExchangeRate {  get; private set; }

        public CreateCurrencyExchangeCommand()
        {

        }

        public CreateCurrencyExchangeCommand(int fromAccountBalanceId, int toAccountBalanceId, decimal debitAmount, DateTime timeOfRate, decimal exchangeRate) : this()
        {
            FromAccountBalanceId = fromAccountBalanceId;
            ToAccountBalanceId = toAccountBalanceId;
            DebitAmount = debitAmount;
            ExchangeRate = exchangeRate;
            TimeOfRate = timeOfRate;
        }
    }
}
