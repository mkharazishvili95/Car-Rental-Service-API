using Car_Rental_Service_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeServices
{
    public interface IFakeAdminService
    {
        bool GetRentById(int rentId);
        IEnumerable<Rent> GetAllRents();
        User GetById(int userId);
        IEnumerable<User> GetAllUsers();
    }
    public class FakeAdminService : IFakeAdminService
    {
        private List<Rent> _rents;
        private List<User> _users;
        private CarRentalStatement _carRentalStatement;
        private List<Car> _cars;
        public FakeAdminService()
        {
            _cars = new List<Car>()
            {
                new Car()
                {
                    Id = 1,
                    Mark = "Mercedes-Benz",
                    Model = "E-200",
                    Age = 2020,
                    Color = "Black",
                    IsAvailable = false
                }
            };
            _carRentalStatement = new CarRentalStatement();
            _carRentalStatement.Id = 1;
            _carRentalStatement.Balance = 100;

            _users = new List<User>()
            {
                new User
                {
                    FirstName = "Mikheil",
                    LastName = "Kharazishvili",
                    Age = 28,
                    Email = "Mkharazishvili1@gmail.com",
                    UserName = "Misho123",
                    Password = "Misho123",
                    IdentityNumber = "01010101010",
                    Balance = 999999999,
                    ContactNumber = "598989898",
                    Role = "User"
                },
                new User
                {
                    FirstName = "Nika",
                    LastName = "Kharazishvili",
                    Age = 27,
                    Email = "Nika333@gmail.com",
                    UserName = "Nika123",
                    Password = "Nika123",
                    IdentityNumber = "11010101010",
                    Balance = 9999999991,
                    ContactNumber = "598989892",
                    Role = "User"
                }
            };
            
            _rents = new List<Rent>
            {
                new Rent()
                {
                    Id = 1,
                    userId = 1,
                    carId = 1,
                    RentFrom = DateTime.Now,
                    RentTo = DateTime.Now.AddDays(1)
                },
                new Rent()
                {
                    Id = 2,
                    userId = 2,
                    carId = 2,
                    RentFrom = DateTime.Now,
                    RentTo = DateTime.Now.AddDays(1)
                }
            };
        }

        public IEnumerable<Rent> GetAllRents()
        {
            if( _rents.Count == 0 )
            {
                return null;
            }
            else
            {
                return _rents;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                var userList = _users.ToList();
                if(userList .Count == 0)
                {
                    return null;
                }
                else
                {
                    return userList;
                }
            }
            catch
            {
                return null;
            }
        }

        public User GetById(int userId)
        {
            try
            {
                var existingUser = _users.SingleOrDefault(user => user.Id == userId);
                if(existingUser == null)
                {
                    return null;
                }
                else
                {
                    return existingUser;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool GetRentById(int rentId)
        {
            try
            {
                var getRentById = _rents.SingleOrDefault(rent => rent.Id == rentId);
                if( getRentById == null )
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
