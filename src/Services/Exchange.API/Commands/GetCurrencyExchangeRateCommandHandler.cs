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
            Currency? fromCurrency = await _currencyRepository.GetCurrencyByIdAsync(command.FromCurrencyId);
            Currency? toCurrency = await _currencyRepository.GetCurrencyByIdAsync(command.ToCurrencyId);

            if (fromCurrency == null) throw new InvalidOperationException($"Currency of id ({command.FromCurrencyId}) was not found.");
            if (toCurrency == null) throw new InvalidOperationException($"Currency of id ({command.ToCurrencyId}) was not found.");

            string fromCurrencyCode = fromCurrency.Code;
            string toCurrencyCode = toCurrency.Code;

            var foundRate = await _cache.GetRecordAsync<ExchangeRate>(ExchangeRate.ToKey(fromCurrencyCode, toCurrencyCode));

            if (foundRate == null)
            {
                foundRate = await _exchangeRateFixerService.GetRateAsync(fromCurrencyCode, toCurrencyCode);

                await _cache.SetRecordAsync(foundRate.Key, foundRate);
            }

            return foundRate;
        }
    }
}
