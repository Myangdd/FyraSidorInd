namespace TeamFyraSidor.Models
{
    public class ElPriceVM
    {
        public required string Date { get; set; }

        public List<int> Hours { get; set; } = new List<int>();

        public List<float> PriceSekSE1 { get; set; } = new List<float>();
        public List<float> PriceSekSE2 { get; set; } = new List<float>();
        public List<float> PriceSekSE3 { get; set; } = new List<float>();
        public List<float> PriceSekSE4 { get; set; } = new List<float>();

    }
}
