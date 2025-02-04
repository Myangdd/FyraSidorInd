using TeamFyraSidor.Models;

namespace TeamFyraSidor.Service
{

    public class ElpriceService: IElpriceService
    {
        private readonly HttpClient _httpClient;

        public ElpriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ElPriceVM> GetElPriceTodayAsync() 
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            var elpriceResponse = await _httpClient.GetAsync($"https://spotprices.lexlink.se/espot/{today}");
            if (elpriceResponse.IsSuccessStatusCode)
            {
                var elPrice = await elpriceResponse.Content.ReadFromJsonAsync<ElPrice>();
                if (elPrice != null)
                {
                    return GetElPriceVM(elPrice);
                }
                else
                {
                    throw new HttpRequestException($"Error fetching el price data: {elpriceResponse.StatusCode}");
                }
            }
            else
            {
                throw new HttpRequestException($"Error fetching el price data: {elpriceResponse.StatusCode}");
            }

        }

        public ElPriceVM GetElPriceVM(ElPrice elPrice) 
        {
            var elPriceVM = new ElPriceVM()
            {
                Date = elPrice.date,
                Hours = elPrice.SE1.Select(x => x.hour).ToList(),
                PriceSekSE1 = elPrice.SE1.Select(x => x.price_sek).ToList(),
                PriceSekSE2 = elPrice.SE2.Select(x => x.price_sek).ToList(),
                PriceSekSE3 = elPrice.SE3.Select(x => x.price_sek).ToList(),
                PriceSekSE4 = elPrice.SE4.Select(x => x.price_sek).ToList()
            };

            return elPriceVM;
        }

    }
}
