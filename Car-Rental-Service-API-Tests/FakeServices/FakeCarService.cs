using System;
using System.Collections.Generic;
using System.Linq;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Services;
using NUnit.Framework;

namespace FakeServices
{
    public interface IFakeCarService
    {
        Car AddNewCar(Car newCar);
        bool UpdateCar(Car updateCar, int carId);
        IEnumerable<Car> GetAllCars();
        IEnumerable<Car> AvailableCars();
        IEnumerable<Car> SortByPrice(double minPrice, double maxPrice);
        IEnumerable<Car> SortByEnginePower(double minPower, double maxPower);
        IEnumerable<Car> GetByMark(string mark);
        IEnumerable<Car> GetByModel(string carModel);
        IEnumerable<Car> GetByColor(string carColor);
        IEnumerable<Car> DetailedSorting(DetailedSorting sorting);
        bool ChangeNotAvailableStatusToAvailable(int carId);
        bool ChangeAvailableStatusToNotAvailable(int carId);
        bool DeleteCar(int carId);
    }
    public class FakeCarService : IFakeCarService
    {
        private List<Car> _cars;

        public FakeCarService()
        {
            _cars = new List<Car>
            {
                new Car {
                    Id = 1,
                    Mark = "Mercedes-Benz",
                    Model = "E-200",
                    Color = "Red",
                    Age = 2020,
                    FuelType = "Diesel",
                    EnginePower = 2.2,
                    Price = 140,
                    IsAvailable = true },
                new Car {
                    Id = 2,
                    Mark = "Toyota",
                    Model = "Camry SE",
                    Color = "Black",
                    Age = 2022,
                    FuelType = "Gas",
                    EnginePower = 3.5,
                    Price = 170,
                    IsAvailable = true }
            };
        }
        public Car AddNewCar(Car newCar)
        {
            try
            {
                if (newCar == null || string.IsNullOrWhiteSpace(newCar.Mark))
                {
                    return null;
                }

                _cars.Add(newCar);
                return newCar;
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
                var existingCar = _cars.Find(car => car.Id == carId);
                if (existingCar == null)
                {
                    return false;
                }

                existingCar.Mark = updateCar.Mark;
                existingCar.Model = updateCar.Model;
                existingCar.Price = updateCar.Price;
                existingCar.Age = updateCar.Age;
                existingCar.EnginePower = updateCar.EnginePower;
                existingCar.Color = updateCar.Color;
                existingCar.FuelType = updateCar.FuelType;
                existingCar.IsAvailable = updateCar.IsAvailable;
                Console.WriteLine("Car updated successfully");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating car: {ex.Message}");
                return false;
            }
        }


        public IEnumerable<Car> GetAllCars()
        {
            return _cars;
        }

        public IEnumerable<Car> AvailableCars()
        {
            return _cars.Where(car => car.IsAvailable);
        }

        public IEnumerable<Car> SortByPrice(double minPrice, double maxPrice)
        {
            return _cars.Where(car => car.Price >= minPrice && car.Price <= maxPrice);
        }

        public IEnumerable<Car> SortByEnginePower(double minPower, double maxPower)
        {
            return _cars.Where(car => car.EnginePower >= minPower && car.EnginePower <= maxPower);
        }

        public IEnumerable<Car> GetByMark(string mark)
        {
            return _cars.Where(car => car.Mark.ToUpper().Contains(mark.ToUpper()));
        }

        public IEnumerable<Car> GetByModel(string carModel)
        {
            return _cars.Where(car => car.Model.ToUpper().Contains(carModel.ToUpper()));
        }

        public IEnumerable<Car> GetByColor(string carColor)
        {
            return _cars.Where(car => car.Color.ToUpper().Contains(carColor.ToUpper()));
        }

        public IEnumerable<Car> DetailedSorting(DetailedSorting sorting)
        {
            return _cars.Where(car =>
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
        }
        public bool ChangeNotAvailableStatusToAvailable(int carId)
        {
            try
            {
                var existingCar = _cars.FirstOrDefault(car => car.Id == carId);
                if (existingCar == null || existingCar.IsAvailable)
                {
                    return false;
                }

                existingCar.IsAvailable = true;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeAvailableStatusToNotAvailable(int carId)
        {
            try
            {
                var existingCar = _cars.FirstOrDefault(car => car.Id == carId);
                if (existingCar == null || !existingCar.IsAvailable)
                {
                    return false;
                }

                existingCar.IsAvailable = false;
                return true;
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
                var existingCar = _cars.FirstOrDefault(car => car.Id == carId);
                if (existingCar == null)
                {
                    return false;
                }

                _cars.Remove(existingCar);
                return true;
            }
            catch
            {
                return false;
            }
        }
    
    }
}
