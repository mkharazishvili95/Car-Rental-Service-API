namespace Car_Rental_Service_API.Models
{
    public class DetailedSorting
    {
        public string Mark { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int minAge { get; set; }
        public int maxAge { get; set; }
        public double EnginePower { get; set; }
        public double minPrice { get; set; }
        public double maxPrice { get; set; }
        public double minPower { get; set;  }
        public double maxPower { get; set; }
    }
}
