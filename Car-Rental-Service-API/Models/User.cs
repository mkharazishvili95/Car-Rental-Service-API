using Car_Rental_Service_API.Identity;

namespace Car_Rental_Service_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public double Balance { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; }
        public User()
        {
            Role = Roles.User;
            Balance = 0;
        }
    }
}
