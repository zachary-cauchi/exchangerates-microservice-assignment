namespace Exchange.API.Services
{
    public interface IExchangeRateService
    {
        public Task<ExchangeRate> GetRateAsync(string fromCurrencyCode, string toCurrencyCode);
    }
}
