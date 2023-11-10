namespace Exchange.API.Models
{
    public class FixerResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        private long _rawTimestamp;
        [JsonPropertyName("timestamp")]
        public long RawTimestamp
        {
            get { return _rawTimestamp; }
            set
            {
                _rawTimestamp = value;
                Timestamp = DateTime.SpecifyKind(DateTimeOffset.FromUnixTimeSeconds(RawTimestamp).DateTime, DateTimeKind.Utc);
                Date = Timestamp.Date;
            }
        }

        public DateTime Timestamp { get; private set; }

        [JsonPropertyName("base")]
        public string Base { get; set; } = null!;

        public DateTime Date { get; private set; }

        [JsonPropertyName("rates")]
        public IDictionary<string, decimal> Rates { get; set; } = new Dictionary<string, decimal>();

        public ErrorDetails Error { get; set; }

        public struct ErrorDetails
        {
            public string Code { get; set; }

            public string Type { get; set; }

            public string Message { get; set; }
        }
    }
}
