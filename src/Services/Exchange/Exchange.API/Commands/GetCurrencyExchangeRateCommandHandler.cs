using Microsoft.Extensions.Caching.Distributed;

namespace Exchange.API.Commands
{
    public class GetCurrencyExchangeRateCommandHandler : IRequestHandler<GetCurrencyExchangeRateCommand, ExchangeRate>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IDistributedCache _cache;
        private readonly IExchangeRateService _exchangeRateFixerService;
        private ILogger<GetCurrencyExchangeRateCommandHandler> _logger;

        public GetCurrencyExchangeRateCommandHandler(ICurrencyRepository currencyRepository, IDistributedCache cache, ILogger<GetCurrencyExchangeRateCommandHandler> logger, IExchangeRateService exchangeRateFixerService)
        {
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _exchangeRateFixerService = exchangeRateFixerService;
        }

        public async Task<ExchangeRate> Handle(GetCurrencyExchangeRateCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling exchange rate command for currencies ({srcCurrencyId}) to ({destCurrencyId})", command.FromCurrencyId, command.ToCurrencyId);

            _logger.LogInformation("Getting currency information for currencies ({srcCurrencyId}) and ({destCurrencyId})", command.FromCurrencyId, command.ToCurrencyId);

            Currency? fromCurrency = await _currencyRepository.GetCurrencyByIdAsync(command.FromCurrencyId);
            Currency? toCurrency = await _currencyRepository.GetCurrencyByIdAsync(command.ToCurrencyId);

            if (fromCurrency == null)
            {
                _logger.LogWarning("Could not find source currency with id ({srcCurrencyId}).", command.FromCurrencyId);
                throw new InvalidOperationException($"Source currency of id ({command.FromCurrencyId}) was not found.");
            }

            if (toCurrency == null)
            {
                _logger.LogWarning("Could not find destination currency with id ({destCurrencyId}).", command.ToCurrencyId);

                throw new InvalidOperationException($"Destination currency of id ({command.ToCurrencyId}) was not found.");
            }

            string fromCurrencyCode = fromCurrency.Code;
            string toCurrencyCode = toCurrency.Code;

            var rateKey = ExchangeRate.ToKey(fromCurrencyCode, toCurrencyCode);
            _logger.LogDebug("Checking cache for exchange rate {rateKey}", rateKey);

            var foundRate = await _cache.GetRecordAsync<ExchangeRate>(rateKey);

            if (foundRate == null)
            {
                _logger.LogDebug("Cached exchange rate ({rateKey}) not found. Getting rate from service.", rateKey);
                
                foundRate = await _exchangeRateFixerService.GetRateAsync(fromCurrencyCode, toCurrencyCode);

                _logger.LogDebug("Saving exchange rate ({rateKy})", rateKey);
                await _cache.SetRecordAsync(foundRate.Key, foundRate);
            }

            _logger.LogInformation("Returning exchange rate with key ({rateKey}).", rateKey);

            return foundRate;
        }
    }
}
