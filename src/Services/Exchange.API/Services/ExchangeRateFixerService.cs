using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NuGet.Protocol;

namespace Exchange.API.Services
{
    public class ExchangeRateFixerService : IExchangeRateFixerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ExchangeRateFixerService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<ExchangeRate> GetRateAsync(string fromCurrencyCode, string toCurrencyCode)
        {
            // This will ideally be replaced with a secrets store like Azure Key Vault
            string apiKey = _config.GetValue<string>("ExchangeRateProviders:Fixer:ApiKey") ?? throw new ArgumentNullException("No Fixer API key was found. Cannot call API.");

            // Using the Basic plan for the API restricts us to only EUR.
            if (fromCurrencyCode != "EUR")
            {
                throw new InvalidOperationException("Currently, only EUR as a source currency is supported.");
            }

            var uri = $"/latest?base={fromCurrencyCode}&symbols={toCurrencyCode}&access_key={apiKey}";
            var response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var rawData = await response.Content.ReadAsStringAsync();
            var data = System.Text.Json.JsonSerializer.Deserialize<FixerResponse>(rawData);

            if (data == null || !data.Success)
            {
                throw new Exception($"Failed to get rates from Fixer for currencies {fromCurrencyCode} to {toCurrencyCode}. Error code: {data?.Error.Type ?? data?.Error.Code}");
            }

            return FromResponse(data, toCurrencyCode);
        }

        public static ExchangeRate FromResponse(FixerResponse response, string toCurrencyCode)
        {
            return new ExchangeRate(response.Base, toCurrencyCode, response.Timestamp, "Fixer", response.Rates[toCurrencyCode]);
        }
    }
}
