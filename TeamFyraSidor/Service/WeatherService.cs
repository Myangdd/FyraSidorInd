namespace TeamFyraSidor.Service;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeamFyraSidor.Models;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _apiKey;


    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
            _httpClient = httpClient;
            _baseUrl = configuration["WeatherApi:BaseUrl"];
            _apiKey = configuration["WeatherApi:ApiKey"]; //Environment.GetEnvironmentVariable("WeatherApiKey");  THIS IS RETRIEVING APIKEY FROM
                                                           //AZURE ENVIRONMENT VARIABLES WHICH IS EVEN SAFER THAN STORING IN APPSETTING.JSON.
    }
    public async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            string endPoint = $"{_baseUrl}weather?q={city}&appid={_apiKey}&units=metric";

            HttpResponseMessage response = await _httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var task = await response.Content.ReadFromJsonAsync<WeatherResponse>();

                return task!;
            }
            else
            {
                throw new HttpRequestException($"Error fetching weather data: {response.StatusCode}");
            }
        }
    }

