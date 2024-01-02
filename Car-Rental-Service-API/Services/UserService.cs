using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Helpers;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Validation;

namespace Car_Rental_Service_API.Services
{
    public interface IUserService
    {
        User RegisterUser(RegisterModel newUser);
        User LoginUser(LoginModel loginUser);
    }
    public class UserService : IUserService
    {
        private readonly CarRentalContext _context;
        public UserService(CarRentalContext context)
        {
            _context = context;
        }

        public User LoginUser(LoginModel loginUser)
        {
            try
            {
                if (string.IsNullOrEmpty(loginUser.UserName) || string.IsNullOrEmpty(loginUser.Password) ||
                    string.IsNullOrEmpty(loginUser.Email))
                {
                    return null;
                }
                var existingUser = _context.Users.FirstOrDefault(user => user.UserName == loginUser.UserName);
                if (existingUser == null)
                {
                    return null;
                }
                if (HashSettings.HashPassword(loginUser.Password) != (existingUser.Password))
                {
                    return null;
                }
                if (loginUser.Email.ToUpper() != existingUser.Email.ToUpper())
                {
                    return null;
                }
                return existingUser;
            }
            catch
            {
                return null;
            }
        }

        public User RegisterUser(RegisterModel registerUser)
        {
            try
            {
                var newUserValidation = new RegisterValidation(_context);
                var validationResults = newUserValidation.Validate(registerUser);
                if (!validationResults.IsValid)
                {
                    return null;
                }
                else
                {
                    var newUser = new User
                    {
                        FirstName = registerUser.FirstName,
                        LastName = registerUser.LastName,
                        IdentityNumber = registerUser.IdentityNumber,
                        ContactNumber = "(+995) "+registerUser.ContactNumber,
                        Email = registerUser.Email,
                        UserName = registerUser.UserName,
                        Password = HashSettings.HashPassword(registerUser.Password),
                        Age = registerUser.Age
                    };
                    _context.Add(newUser);
                    _context.SaveChanges();
                    return (newUser);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
