using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Helpers;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Car_Rental_Service_API.Services
{
    public interface ICarService
    {
        IEnumerable<Car> GetAllCars();
        IEnumerable<Car> AvailableCars();
        IEnumerable<Car> SortByPrice(double minPrice, double maxPrice);
        IEnumerable<Car> SortByEnginePower(double minPower, double maxPower);
        IEnumerable<Car> GetByMark(string mark);
        IEnumerable<Car> GetByModel(string carModel);
        IEnumerable<Car> GetByColor(string carColor);
        IEnumerable<Car> DetailedSorting(DetailedSorting sorting);
        Car AddNewCar(Car newCar);
        bool UpdateCar(Car updateCar, int carId);
        bool ChangeNotAvailableStatusToAvailable(int carId);
        bool ChangeAvailableStatusToNotAvailable(int carId);
        bool DeleteCar(int carId);
    }
    public class CarService : ICarService
    {
        private readonly CarRentalContext _context;
        public CarService(CarRentalContext context) 
        {
            _context = context;
        }

        public Car AddNewCar(Car newCar)
        {
            try
            {
                var carValidation = new CarValidation();
                var validationResults = carValidation.Validate(newCar);
                if (!validationResults.IsValid)
                {
                    return null;
                }
                else
                {
                    var addNewCar = new Car
                    {
                        Mark = newCar.Mark,
                        Model = newCar.Model,
                        Color = newCar.Color,
                        Age = newCar.Age,
                        FuelType = newCar.FuelType,
                        EnginePower = newCar.EnginePower,
                        Price = newCar.Price
                    };
                    _context.Add(newCar);
                    _context.SaveChanges();
                    return addNewCar;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Car> AvailableCars()
        {
            try
            {
                var availableCars = _context.Cars.Where(cars => cars.IsAvailable == true).ToList();
                if(availableCars == null || availableCars.Count == 0)
                {
                    return null;
                }
                else
                {
                    return availableCars;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool ChangeAvailableStatusToNotAvailable(int carId)
        {
            try
            {
                var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
                if (existingCar == null)
                {
                    return false;
                }
                if(existingCar.IsAvailable == false)
                {
                    return false;
                }
                if(existingCar.IsAvailable == true)
                {
                    existingCar.IsAvailable = false;
                    _context.Update(existingCar);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeNotAvailableStatusToAvailable(int carId)
        {
            try
            {
                var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
                if (existingCar == null)
                {
                    return false;
                }
                if(existingCar.IsAvailable == true)
                {
                    return false;
                }
                if(existingCar.IsAvailable == false)
                {
                    existingCar.IsAvailable = true;
                    _context.Update(existingCar);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCar(int carId)
        {
            try
            {
                var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
                if (existingCar == null)
                {
                    return false;
                }
                else
                {
                    _context.Remove(existingCar);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            
        }

        public IEnumerable<Car> DetailedSorting(DetailedSorting sorting)
        {
            try
            {
                var detailedSorting = _context.Cars.Where(car =>
                    car.Mark.ToUpper().Contains(sorting.Mark.ToUpper()) &&
                    car.Model.ToUpper().Contains(sorting.Model.ToUpper()) &&
                    car.Color.ToUpper().Contains(sorting.Color.ToUpper()) &&
                    car.EnginePower >= sorting.minPower &&
                    car.EnginePower <= sorting.maxPower &&
                    car.Price >= sorting.minPrice &&
                    car.Price <= sorting.maxPrice &&
                    car.Age >= sorting.minAge &&
                    car.Age <= sorting.maxAge)
                    .ToList();

                return detailedSorting;
            }
            catch
            {
                return Enumerable.Empty<Car>();
            }
        }


        public IEnumerable<Car> GetAllCars()
        {
            try
            {
                var carList = _context.Cars.ToList();
                if(carList.Count == 0)
                {
                    return null;
                }
                else
                {
                    return carList;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Car> GetByColor(string carColor)
        {
            try
            {
                var sortByColor = _context.Cars.Where(car => car.Color.Contains(carColor.ToUpper())).ToList();
                if(sortByColor == null || sortByColor.Count == 0)
                {
                    return null;
                }
                else
                {
                    return sortByColor;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Car> GetByMark(string mark)
        {
            try
            {
                var getByMark = _context.Cars.Where(car => car.Mark.ToUpper().Contains(mark.ToUpper())).ToList();
                if(getByMark.Count == 0)
                {
                    return null;
                }
                else
                {
                    return getByMark;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Car> GetByModel(string carModel)
        {
            try
            {
                var sortByModel = _context.Cars.Where(car => car.Model.ToUpper().Contains(carModel.ToUpper())).ToList();
                if(sortByModel == null || sortByModel.Count == 0)
                {
                    return null;
                }
                else
                {
                    return sortByModel;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Car> SortByEnginePower(double minPower, double maxPower)
        {
            try
            {
                var sortedByEnginePower = _context.Cars.Where(car => car.EnginePower >= minPower && car.EnginePower <= maxPower).ToList();
                if (sortedByEnginePower == null || sortedByEnginePower.Count == 0)
                {
                    return null;
                }
                else
                {
                    return sortedByEnginePower;
                }
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Car> SortByPrice(double minPrice, double maxPrice)
        {
            try
            {
                var sortedCars = _context.Cars.Where(car => car.Price >= minPrice && car.Price <= maxPrice).ToList();
                if(sortedCars == null || sortedCars.Count == 0)
                {
                    return null;
                }
                else
                {
                    return sortedCars;
                }
            }
            catch
            {
                return null;
            }
            
        }

        public bool UpdateCar(Car updateCar, int carId)
        {
            try
            {
                var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
                if(existingCar == null)
                {
                    return false;
                }
                else
                {
                    var carValidation = new CarValidation();
                    var validationResults = carValidation.Validate(updateCar);
                    if (!validationResults.IsValid)
                    {
                        return false;
                    }
                    else
                    {
                        existingCar.Mark = updateCar.Mark;
                        existingCar.Model = updateCar.Model;
                        existingCar.Price = updateCar.Price;
                        existingCar.Age = updateCar.Age;
                        existingCar.EnginePower = updateCar.EnginePower;
                        existingCar.Color = updateCar.Color;
                        existingCar.FuelType = updateCar.FuelType;
                        existingCar.IsAvailable = updateCar.IsAvailable;
                        _context.Update(existingCar);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
