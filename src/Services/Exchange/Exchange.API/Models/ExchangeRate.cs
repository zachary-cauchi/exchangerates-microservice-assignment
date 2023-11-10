namespace Exchange.API.Models
{
    public class ExchangeRate
    {
        [JsonPropertyName("key")]
        public string Key { get; set; } = null!;

        [JsonPropertyName("srcCurrencyCode")]
        public string SrcCurrencyCode { get; set; } = null!;

        [JsonPropertyName("destCurrencyCode")]
        public string DestCurrencyCode { get; set; } = null!;

        [JsonPropertyName("timeAccessed")]
        public DateTime TimeAccessed { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; } = null!;

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }

        public ExchangeRate()
        {

        }

        public ExchangeRate(string srcCurrencyCode, string destCurrencyCode, DateTime timeAccessed, string source, decimal rate)
        {
            SrcCurrencyCode = srcCurrencyCode ?? throw new ArgumentNullException(nameof(srcCurrencyCode));
            DestCurrencyCode = destCurrencyCode ?? throw new ArgumentNullException(nameof(destCurrencyCode));
            Key = ToKey(srcCurrencyCode, destCurrencyCode);
            TimeAccessed = timeAccessed;
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Rate = rate;
        }

        public static string ToKey(string fromCurrencyCode, string toCurrencyCode)
        {
            return fromCurrencyCode + "_" + toCurrencyCode;
        }
    }
}
