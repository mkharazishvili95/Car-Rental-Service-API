using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Models;
using Tweetinvi.Models;

namespace Car_Rental_Service_API.Services
{
    public interface IAdminService
    {
        IEnumerable<User> GetAllUsers();
        User GetById(int userId);
        bool MakeRefund(MakeARefund refundModel);
        bool CancelRentWithoutRefund(int rentId);
        bool rentCancelation(int rentId);
        bool GetRentById(int rentId);
        IEnumerable<Rent> GetAllRents();
    }
    public class AdminService : IAdminService
    {
        private readonly CarRentalContext _context;
        public AdminService(CarRentalContext context)
        {
            _context = context;
        }

        public bool CancelRentWithoutRefund(int rentId)
        {
            try
            {
                var existingRent = _context.Rents.SingleOrDefault(rent => rent.Id == rentId);
                if(existingRent == null)
                {
                    return false;
                }
                var existingCar = _context.Cars.SingleOrDefault(car => car.Id == existingRent.carId);
                if (existingCar == null)
                {
                    return false;
                }
                else
                {
                    _context.Remove(existingRent);
                    existingCar.IsAvailable = true;
                    _context.Update(existingCar);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable <Rent> GetAllRents()
        {
            try
            {
                var rentList = _context.Rents.ToList();
                if(rentList.Count == 0)
                {
                    return null;
                }
                else
                {
                    return rentList;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                var userList = _context.Users.ToList();
                if(userList.Count == 0)
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
                var existingUser = _context.Users.SingleOrDefault(user => user.Id == userId);
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
                var getRentById = _context.Rents.SingleOrDefault(rent => rent.Id == rentId);
                if (getRentById == null)
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

        public bool MakeRefund(MakeARefund refundModel)
        {
            try
            {
                var existingUser = _context.Users.SingleOrDefault(user => user.Id == refundModel.userId);
                if(existingUser == null)
                {
                    return false;
                }
                var companyStatement = _context.CarRentalStatement.SingleOrDefault() ?? new CarRentalStatement();
                if(companyStatement.Balance < refundModel.Amount)
                {
                    return false;
                }
                else
                {
                    existingUser.Balance += refundModel.Amount;
                    companyStatement.Balance -= refundModel.Amount;
                    _context.Update(existingUser);
                    _context.Update(companyStatement);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool rentCancelation(int rentId)
        {
            try
            {
                var existingRent = _context.Rents.SingleOrDefault(rent => rent.Id == rentId);
                if (existingRent == null)
                {
                    return false;
                }

                var existingCar = _context.Cars.SingleOrDefault(car => car.Id == existingRent.carId);
                var existingUser = _context.Users.SingleOrDefault(user => user.Id == existingRent.userId);

                if (existingCar == null || existingUser == null)
                {
                    return false;
                }
                else
                {
                    var companyStatement = _context.CarRentalStatement.SingleOrDefault() ?? new CarRentalStatement();
                    TimeSpan rentalDuration = existingRent.RentTo - existingRent.RentFrom;
                    int rentalDurationInDays = (int)rentalDuration.TotalDays;
                    double rentalCost = existingCar.Price * rentalDurationInDays; 
                    existingUser.Balance += rentalCost;
                    companyStatement.Balance -= rentalCost;
                    existingCar.IsAvailable = true;
                    _context.Update(existingCar);
                    _context.Update(companyStatement);
                    _context.Update(existingUser);
                    _context.Remove(existingRent);
                    _context.SaveChanges();
                    _context.SaveChanges();
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
