namespace Exchange.API.Services
{
    public interface IExchangeRateFixerService
    {
        public Task<ExchangeRate> GetRateAsync(string fromCurrencyCode, string toCurrencyCode);
    }
}
