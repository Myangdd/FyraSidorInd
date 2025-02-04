using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using TeamFyraSidor.Models;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.ViewComponents
{
    public class StockViewComponent : ViewComponent
    {
        private readonly IStockService _stockService;

        public StockViewComponent(IStockService stockService)
        {
            _stockService = stockService;
        }

        public async Task <IViewComponentResult> InvokeAsync(string symbol)
        {
            try
            {
                var stockResponse = await _stockService.GetStockResponseAsync(symbol);

                var stockViewModel = new StockViewModel
                {
                    Symbol = stockResponse.Symbol,
                    Open = stockResponse.Open,
                    High = stockResponse.High
                    //Change = stockResponse.Change,
                    //ChangePercent = stockResponse.ChangePercent
                };

                return View(stockViewModel);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = $"Error: {ex.Message}";
                return View("Error");
            }
           
        }

    }
}
