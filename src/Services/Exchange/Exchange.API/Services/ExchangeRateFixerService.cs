namespace Exchange.API.Services
{
    public class ExchangeRateFixerService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<ExchangeRateFixerService> _logger;

        public ExchangeRateFixerService(HttpClient httpClient, IConfiguration config, ILogger<ExchangeRateFixerService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ExchangeRate> GetRateAsync(string fromCurrencyCode, string toCurrencyCode)
        {
            _logger.LogInformation("Getting currency exchange rate from currency ({currencyCode}) to currency ({currencyCode})", fromCurrencyCode, toCurrencyCode);

            // This will ideally be replaced with a secrets store like Azure Key Vault
            string apiKey = _config.GetValue<string>("ExchangeRateProviders:Fixer:ApiKey") ?? throw new ArgumentNullException("No Fixer API key was found. Cannot call API.");

            // Using the Basic plan for the API restricts us to only EUR.
            if (fromCurrencyCode != "EUR")
            {
                _logger.LogWarning("Request to convert from non-EUR currency ({fromCurrencyCode}) was rejected.", fromCurrencyCode);

                throw new InvalidOperationException("Currently, only EUR as a source currency is supported.");
            }

            _logger.LogDebug("Sending exchange rate request to provider ({url}).", _httpClient.BaseAddress);
            var uri = $"/latest?base={fromCurrencyCode}&symbols={toCurrencyCode}&access_key={apiKey}";
            var response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            var rawData = await response.Content.ReadAsStringAsync();
            var data = System.Text.Json.JsonSerializer.Deserialize<FixerResponse>(rawData);

            if (data == null || !data.Success)
            {
                _logger.LogWarning("Received non-success response from provider with error code ({code})", data?.Error.Code);

                throw new Exception($"Failed to get rates from Fixer for currencies {fromCurrencyCode} to {toCurrencyCode}. Error code: {data?.Error.Type ?? data?.Error.Code}");
            }

            _logger.LogInformation("Returning new exchange rate for currency ({currencyCode}) to currency ({currencyCode})", fromCurrencyCode, toCurrencyCode);

            return FromResponse(data, toCurrencyCode);
        }

        public static ExchangeRate FromResponse(FixerResponse response, string toCurrencyCode)
        {
            return new ExchangeRate(response.Base, toCurrencyCode, response.Timestamp, "Fixer", response.Rates[toCurrencyCode]);
        }
    }
}
