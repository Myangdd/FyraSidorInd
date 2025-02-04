
using System.Net.Http;
using System.Net.Http.Json;
using System.ClientModel.Primitives;
using TeamFyraSidor.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace TeamFyraSidor.Service
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://www.alphavantage.co/query";
        private const string ApiKey = "N7PCFZZK096LG10Y";
        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [ResponseCache(CacheProfileName = "apicache")]
        public async Task<GlobalQuote> GetStockResponseAsync(string symbol)
        {
            var endPoint = $"{BaseUrl}?function=GLOBAL_QUOTE&symbol={symbol}&apikey={ApiKey}";

            var response = await _httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                if (json.Contains("Information") || json.Contains("Error"))
                {
                    throw new HttpRequestException($"API error: {json}");
                }

                var stockResponse = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
                if (stockResponse != null && stockResponse.TryGetValue("Global Quote", out var globalQuoteJson))
                {
                    var globalQuote = JsonSerializer.Deserialize<GlobalQuote>(globalQuoteJson.GetRawText());
                    if (globalQuote != null)
                    {
                        return globalQuote;
                    }
                }

                throw new HttpRequestException("Invalid response structure from API.");
            }

            throw new HttpRequestException($"Error fetching stock data: {response.StatusCode}");
        }
        //public async Task<GlobalQuote> GetStockResponseAsync(string symbol)
        //{
        //    var endPoint = $"{BaseUrl}?function=GLOBAL_QUOTE&symbol={symbol}&apikey={ApiKey}";

        //    var response = await _httpClient.GetAsync(endPoint);



        //        var json = await response.Content.ReadAsStringAsync();
        //        if (json.Contains("Information"))
        //        {
        //            throw new HttpRequestException("API limit reached: " + json);
        //        }
        //        else
        //        {
        //            var stockResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, GlobalQuote>>();

        //            return stockResponse["Global Quote"];
        //        }






        //}
    }
}

