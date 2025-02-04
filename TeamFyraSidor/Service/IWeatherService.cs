using TeamFyraSidor.Models;

namespace TeamFyraSidor.Service
{
    public interface IWeatherService
    {
        public Task<WeatherResponse> GetWeatherAsync(string city);
    }
}
