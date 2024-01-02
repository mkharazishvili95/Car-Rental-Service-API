using Car_Rental_Service_API.Helpers;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Services;
using System.Collections.Generic;

namespace FakeServices
{
    public class FakeUserService : IUserService
    {
        private readonly List<User> _fakeUsers;

        public FakeUserService()
        {
            _fakeUsers = new List<User>();
        }

        public User RegisterUser(RegisterModel newUser)
        {
            var user = new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Age = newUser.Age,
                Email = newUser.Email,
                ContactNumber = newUser.ContactNumber,
                IdentityNumber = newUser.IdentityNumber,
                UserName = newUser.UserName,
                Password = HashSettings.HashPassword(newUser.Password),
            };
            _fakeUsers.Add(user);
            return user;
        }

        public User LoginUser(LoginModel loginUser)
        {
            var user = _fakeUsers.Find(user => user.UserName == loginUser.UserName);
            return user;
        }
    }
}
