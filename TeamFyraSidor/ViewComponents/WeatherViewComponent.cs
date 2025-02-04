using Microsoft.AspNetCore.Mvc;
using TeamFyraSidor.Models;
using TeamFyraSidor.Service;


namespace TeamFyraSidor.ViewComponents
{
    public class WeatherViewComponent : ViewComponent
    {
        private readonly IWeatherService _weatherService;

        public WeatherViewComponent(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string city = "Linköping") 
        {
           
            
                try
                {
                    var weather = await _weatherService.GetWeatherAsync(city);

                    var viewModel = new WeatherViewModel
                    {
                        Name = weather.Name,
                        Temp = weather.Main.Temp,
                        Humidity = weather.Main.Humidity,
                        WindSpeed = weather.Wind.Speed,
                        Icon = $"http://openweathermap.org/img/wn/{ weather.Weather.FirstOrDefault()?.Icon}.png" ?? "Icon Not Found",
                        Description = weather.Weather.FirstOrDefault()?.Description ?? "Description Not Found."
                    };

                    return View(viewModel);
                }
                catch (Exception ex)
                {

                    ViewData["Error"] = $"Error: {ex.Message}";
                    return View();
                }

            
           
        }


    }
}
