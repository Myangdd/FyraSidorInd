using System.ComponentModel.DataAnnotations;

namespace TeamFyraSidor.Models
{
    public class ElPrice
    {
      
        public required string date { get; set; }
        public required SECategory[] SE1 { get; set; }
        public required SECategory[] SE2 { get; set; }
        public required SECategory[] SE3 { get; set; }
        public required SECategory[] SE4 { get; set; }

        public class SECategory
        {
            public int hour { get; set; }
            public float price_eur { get; set; }
            public float price_sek { get; set; }
            public int kmeans { get; set; }
        }


    }
}
