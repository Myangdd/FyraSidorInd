namespace TeamFyraSidor.Models
{
    public class WeatherForecast
    {
        public int Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public decimal TempratureC {  get; set; }
        public decimal TempratureF { get; set; }
        public decimal Humidity {  get; set; }
        public decimal WindSpeed { get; set; }
        public DateTime Date { get; set; }
        public required string City { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
