using Microsoft.AspNetCore.Mvc;
using TeamFyraSidor.Models;
using TeamFyraSidor.Service;

namespace TeamFyraSidor.ViewComponents
{
    public class ElPrice : ViewComponent
    {
        private readonly IElpriceService _elpriceService;

        public ElPrice(IElpriceService elpriceService)
        {
            _elpriceService = elpriceService;
        }

        public IViewComponentResult Invoke()
        {
            //try
            //{
            //    var elprice = await _elpriceService.GetElPriceTodayAsync();

            //    return View(elprice);
            //}
            //catch (Exception ex)
            //{
            //    ViewData["Error"] = $"Error: {ex.Message}";
            //    return View();
            //}

            return View();

        }
    }
}
