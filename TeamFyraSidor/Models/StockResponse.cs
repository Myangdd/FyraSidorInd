using System.Text.Json.Serialization;

namespace TeamFyraSidor.Models
{
    public class GlobalQuote
    {
        [JsonPropertyName("01. symbol")]
        public required string Symbol { get; set; }

        [JsonPropertyName("02. open")]
        public string Open { get; set; } = string.Empty;

        [JsonPropertyName("03. high")]
        public string High { get; set; } = string.Empty;

        [JsonPropertyName("04. low")]
        public string Low { get; set; } = string.Empty;

        [JsonPropertyName("05. price")]
        public string Price { get; set; } = string.Empty;

        [JsonPropertyName("06. volume")]
        public string Volume { get; set; } = string.Empty;

        [JsonPropertyName("07. latest trading day")]
        public string LatestTradingDay { get; set; } = string.Empty;

        [JsonPropertyName("08. previous close")]
        public string PreviousClose { get; set; } = string.Empty;

        [JsonPropertyName("09. change")]
        public string Change { get; set; } = string.Empty;

        [JsonPropertyName("10. change percent")]
        public string ChangePercent { get; set; } = string.Empty;
    }
}
