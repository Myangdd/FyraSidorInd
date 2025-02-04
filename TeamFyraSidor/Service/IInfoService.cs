using Azure.Data.Tables;
using TeamFyraSidor.Models.TableData;


namespace TeamFyraSidor.Service
{
    public interface IInfoService
    {
        public List<Info> GetInfo();
        List<ElPriceEntity> GetElPriceFrAzureTable();
    }
}
