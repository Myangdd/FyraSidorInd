using TeamFyraSidor.Models;

namespace TeamFyraSidor.Service
{
    public interface IElpriceService
    {
        Task<ElPriceVM> GetElPriceTodayAsync();
        ElPriceVM GetElPriceVM(ElPrice elPrice);
    }
}
