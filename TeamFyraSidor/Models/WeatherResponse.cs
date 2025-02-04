namespace TeamFyraSidor.Models
{
    public class WeatherResponse
    {
        public required MainInfo Main { get; set; }
        public required WindInfo Wind { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<WeatherInfo> Weather { get; set; } = new List<WeatherInfo>();

    }
    public class MainInfo
    {

        public double Temp { get; set; }
        public int Humidity { get; set; }

    }
    public class WindInfo
    {
        public double Speed { get; set; }
    }
    public class WeatherInfo
    {
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
