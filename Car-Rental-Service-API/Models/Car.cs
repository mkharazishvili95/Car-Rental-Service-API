using System.Data.SqlTypes;

namespace Car_Rental_Service_API.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Mark { get; set; } = string.Empty;
        public string Model { get; set;} = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Age { get; set; }
        public double EnginePower {  get; set; }
        public string FuelType { get; set; } = string.Empty;
        public double Price {  get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
