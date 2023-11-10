namespace Exchange.API.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _repository;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(ICurrencyRepository repository, ILogger<CurrencyService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = _logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Currency?> GetCurrencyByIdAsync(int id)
        {
            _logger.LogInformation("Getting currency with id ({id}).", id);

            return await _repository.GetCurrencyByIdAsync(id);
        }
    }
}
