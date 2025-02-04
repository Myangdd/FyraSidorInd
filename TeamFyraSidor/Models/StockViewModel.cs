namespace TeamFyraSidor.Models
{
    public class StockViewModel
    {
        public required string Symbol { get; set; }
        public string Open { get; set; } = string.Empty;
        public string High {  get; set; } = string.Empty;
        public string Change { get; set; } = string.Empty;
        public string ChangePercent { get; set; } = string.Empty;

    }
}
