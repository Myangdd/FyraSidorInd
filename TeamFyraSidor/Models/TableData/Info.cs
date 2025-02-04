using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TeamFyraSidor.Models.TableData
{
    public class Info : ITableEntity
    { 
        public string Name { get; set; } = default!;
        public double Temp { get; set; } = default;
        public int Humidity { get; set; } = default!;
        public double WindSpeed { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string RowKey { get; set; } = default!;
        public string PartitionKey { get; set; } = default!;
        public ETag ETag { get; set; } = default!;
        public DateTimeOffset? Timestamp { get; set; } = default!;

        //public string Name { get; set; } = default!;
        //public double Temp { get; set; } = default;
        //public int Humidity { get; set; } = default!;
        //public double WindSpeed { get; set; } = default!;
        //public string Description { get; set; } = default!;
    }
}
