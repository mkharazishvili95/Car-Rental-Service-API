namespace Car_Rental_Service_API.Models
{
    public class Rent
    {
        public int Id { get; set; } 
        public int userId {  get; set; }
        public int carId {  get; set; }
        public DateTime RentFrom { get; set; }
        public DateTime RentTo { get; set;}
    }
}
