using Car_Rental_Service_API.Helpers;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Services;
using NUnit.Framework;

namespace FakeServices
{
    [TestFixture]
    public class FakeUserServiceTests
    {
        private IUserService _fakeUserService;

        [SetUp]
        public void Setup()
        {
            _fakeUserService = new FakeUserService();
        }

        [Test]
        public void RegisterUser_ValidUser_ReturnsUser()
        {
            var registerModel = new RegisterModel
            {
                FirstName = "Mikheil",
                LastName = "Kharazishvili",
                Age = 28,
                Email = "Mkharazishvili1@gmail.com",
                ContactNumber = "511111111",
                IdentityNumber = "12345678910",
                UserName = "misho123",
                Password = "misho123",
            };

            var registeredUser = _fakeUserService.RegisterUser(registerModel);
            Assert.IsNotNull(registeredUser);
            Assert.AreEqual(registerModel.FirstName, registeredUser.FirstName);
            Assert.AreEqual(registerModel.LastName, registeredUser.LastName);
            Assert.AreEqual(registerModel.Age, registeredUser.Age);
            Assert.AreEqual(registerModel.ContactNumber, registeredUser.ContactNumber);
            Assert.AreEqual(registerModel.IdentityNumber, registeredUser.IdentityNumber);
            Assert.AreEqual(registerModel.Email, registeredUser.Email);
            Assert.AreEqual(registerModel.UserName, registeredUser.UserName);
        }

        [Test]
        public void LoginUser_ExistingUser_ReturnsUser()
        {
            var loginModel = new LoginModel
            {
                Email = "Mkharazishvili1@gmail.com",
                UserName = "misho123",
                Password = "misho123",
            };

            var registerModel = new RegisterModel
            {
                FirstName = "Mikheil",
                LastName = "Kharazishvili",
                Age = 28,
                Email = "Mkharazishvili1@gmail.com",
                ContactNumber = "511111111",
                IdentityNumber = "12345678910",
                UserName = "misho123",
                Password = "misho123",
            };

            _fakeUserService.RegisterUser(registerModel);

            var loggedInUser = _fakeUserService.LoginUser(loginModel);
            Assert.IsNotNull(loggedInUser);
            Assert.AreEqual(loginModel.UserName, loggedInUser.UserName);
        }

        [Test]
        public void LoginUser_NonExistingUser_ReturnsNull()
        {
            var loginModel = new LoginModel
            {
                Email = "FakeUser@gmail.com",
                UserName = "FakeUser123",
                Password = "FakePassword123",
            };

            var loggedInUser = _fakeUserService.LoginUser(loginModel);
            Assert.IsNull(loggedInUser);
        }
    }
}
