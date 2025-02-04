using Azure.Data.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TeamFyraSidor.Models;
using TeamFyraSidor.Models.TableData;

namespace TeamFyraSidor.Service
{
    public class InfoService : IInfoService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connString;
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableClient _tableClient;
        public InfoService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connString = _configuration["AzureBlobConnectionString"]!;
            _tableServiceClient = new TableServiceClient(_connString);
            //_tableClient = _tableServiceClient.GetTableClient(tableName:"APITable");
            _tableClient = new TableClient(_configuration["AzureBlobConnectionString"], "APITable");
        }
        
        public List<Info> GetInfo()
        {
            var entities = _tableClient.Query<Info>()
                .Where(x => x.PartitionKey == "WeatherData")
                .OrderByDescending(x => x.Timestamp).ToList();
            return entities;
        }
        
        public List<ElPriceEntity> GetElPriceFrAzureTable()
        {
            var entities = _tableClient.Query<ElPriceEntity>()
                .Where(x => x.RowKey.StartsWith("SE")).OrderByDescending(x => x.Timestamp).ToList();
            return entities;
        }
    }
}
