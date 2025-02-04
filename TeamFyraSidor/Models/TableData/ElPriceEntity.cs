using Azure;
using Azure.Data.Tables;

namespace TeamFyraSidor.Models.TableData
{
    public class ElPriceEntity:ITableEntity
    {
        public ETag ETag { get; set; } = default;

        //RowKey to be area_hour: SE1_0, SE2_23
        public string RowKey { get; set; } = default!;

        // partitionkey to be Date, eg.2025-01-19
        public string PartitionKey { get; set; } = default!;

        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.Now;

        public double Price { get; set; } = default;
    }
}
