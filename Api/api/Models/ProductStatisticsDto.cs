namespace api.Models
{
    public class ProductStatisticsDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public double Quantity{ get; set; }
        public double Profit { get; set; }
    }
}