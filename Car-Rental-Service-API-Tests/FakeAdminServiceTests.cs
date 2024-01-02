using Car_Rental_Service_API.Models;
using Microsoft.Identity.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeServices
{
    [TestFixture]
    public class FakeAdminServiceTests
    {
        private IFakeAdminService _fakeAdminService;
        private List<User> _users;
        private List<Car> _cars;
        private CarRentalStatement _carRentalStatement;

        [SetUp]
        public void SeTup()
        {
            _fakeAdminService = new FakeAdminService();
            _users = new List<User>();
            _cars = new List<Car>();
            
        }
        [Test]
        public void GetAllUsers_ReturnsAllUsers()
        {
            IEnumerable<User> allUsers = _fakeAdminService.GetAllUsers();
            Assert.IsNotNull(allUsers);
            Assert.AreEqual(2,allUsers.Count());
        }
        [Test]
        public void GetAllRents_ReturnsAllRents()
        {
            IEnumerable<Rent> allrents = _fakeAdminService.GetAllRents();
            Assert.IsNotNull(allrents);
            Assert.AreEqual(2, allrents.Count());
            Assert.AreNotEqual(3, allrents.Count());
        }
        [Test]
        public void GetRentById_ReturnsRent()
        {
            bool GetRentById = _fakeAdminService.GetRentById(1);
            Assert.IsTrue(GetRentById);
        }
        [Test]
        public void GetUserById_ReturnsUser()
        {
            var userId = 1;
            var existingUser = _fakeAdminService.GetById(1);
            Assert.IsNotNull(1);
        }
    }
}
