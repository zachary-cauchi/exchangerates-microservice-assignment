using System.Runtime.Serialization;

namespace Exchange.API.Commands
{
    public class GetCurrencyExchangeRateCommand : IRequest<ExchangeRate>
    {
        [DataMember]
        public int FromCurrencyId { get; private set; }

        [DataMember]
        public int ToCurrencyId { get; private set; }

        public GetCurrencyExchangeRateCommand()
        {

        }

        public GetCurrencyExchangeRateCommand(int fromCurrencyId, int toCurrencyId)
        {
            FromCurrencyId = fromCurrencyId;
            ToCurrencyId = toCurrencyId;
        }
    }
}
