using TeamFyraSidor.Models;

namespace TeamFyraSidor.Service
{
    public interface IStockService
    {
        public Task<GlobalQuote> GetStockResponseAsync(string symbol);
    }
}
