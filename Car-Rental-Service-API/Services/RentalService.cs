using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Models;

namespace Car_Rental_Service_API.Services
{
    public interface IRentalService
    {
        Car RentACar(int carId, int userId, int companyId, RentalService rentACar);
    }
    public class RentalService : IRentalService
    {
        private readonly CarRentalContext _context;
        public RentalService(CarRentalContext context) 
        {
            _context = context;
        }

        public Car RentACar(int carId, int userId, int companyId, RentalService rentACar)
        {
            try
            {
                var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
                if (existingCar == null)
                {
                    return null;
                }
                if(existingCar.IsAvailable == false)
                {
                    return null;
                }
                var existingUser = _context.Users.SingleOrDefault(user => user.Id == userId);
                if(existingCar == null)
                {
                    return null;
                }
                
                if(existingUser.Balance >= existingCar.Price)
                {
                    existingCar.IsAvailable = false;
                    existingUser.Balance -= existingCar.Price;
                    var companyStatement = _context.CarRentalStatement.FirstOrDefault(c => c.Id == companyId);
                    companyStatement.Balance += existingCar.Price;
                    _context.Update(companyStatement);
                    _context.Update(existingUser);
                    _context.Cars.Update(existingCar);
                    _context.Add(rentACar);
                    _context.SaveChanges();

                }
                return existingCar;
            }
            catch
            {
                return null;
            }
        }
    }
}
