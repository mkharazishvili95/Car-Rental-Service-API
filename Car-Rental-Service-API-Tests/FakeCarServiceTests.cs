using Car_Rental_Service_API.Models;
using FakeServices;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FakeServices
{
    [TestFixture]
    public class FakeCarServiceTests
    {
        private IFakeCarService _fakeCarService;

        [SetUp]
        public void Setup()
        {
            _fakeCarService = new FakeCarService();
        }
        [Test]
        public void AddNewCar_WhenValidCarProvided_ShouldAddToDatabase()
        {
            var newCar = new Car
            {
                Mark = "Toyota",
                Model = "Corolla",
                Price = 25000,
                Age = 2,
                EnginePower = 150,
                Color = "Blue",
                FuelType = "Gasoline",
                IsAvailable = true
            };

            var addedCar = _fakeCarService.AddNewCar(newCar);
            Assert.IsNotNull(addedCar);
        }

        [Test]
        public void UpdateCar_WhenExistingCarUpdated_ShouldModifyDatabaseEntry()
        {
            var existingCar = new Car
            {
                Id = 1,
                Mark = "Toyota",
                Model = "Corolla",
                Price = 170,
                Age = 2022,
                EnginePower = 2.0,
                Color = "Blue",
                FuelType = "Gasoline",
                IsAvailable = true
            };
            _fakeCarService.AddNewCar(existingCar);

            var updatedCar = new Car
            {
                Id = 1,
                Mark = "Toyota",
                Model = "Camry SE",
                Price = 230,
                Age = 2023,
                EnginePower = 2.5,
                Color = "Grey",
                FuelType = "Gasoline",
                IsAvailable = true
            };

            var isUpdated = _fakeCarService.UpdateCar(updatedCar, updatedCar.Id);
            Assert.IsTrue(isUpdated);
        }

        [Test]
        public void GetAllCars_ReturnsAllCars()
        {
            IEnumerable<Car> allCars = _fakeCarService.GetAllCars();
            Assert.IsNotNull(allCars);
            Assert.AreEqual(2, allCars.Count());
        }

        [Test]
        public void AvailableCars_ReturnsAvailableCars()
        {
            IEnumerable<Car> availableCars = _fakeCarService.AvailableCars();
            Assert.IsNotNull(availableCars);
            Assert.AreEqual(2, availableCars.Count());
        }

        [Test]
        public void SortByPrice_ReturnsCarsWithinPriceRange()
        {
            IEnumerable<Car> carsInRange = _fakeCarService.SortByPrice(100, 200);
            Assert.IsNotNull(carsInRange);
            Assert.AreEqual(2, carsInRange.Count());
        }

        [Test]
        public void SortByEnginePower_ReturnsCarsWithinPowerRange()
        {
            IEnumerable<Car> carsInPowerRange = _fakeCarService.SortByEnginePower(2.0, 4.0);
            Assert.IsNotNull(carsInPowerRange);
            Assert.AreEqual(2, carsInPowerRange.Count());
        }

        [Test]
        public void GetByMark_ReturnsCarsByMark()
        {
            IEnumerable<Car> carsByMark = _fakeCarService.GetByMark("Toyota");
            Assert.IsNotNull(carsByMark);
            Assert.AreEqual(1, carsByMark.Count());
        }

        [Test]
        public void GetByModel_ReturnsCarsByModel()
        {
            IEnumerable<Car> carsByModel = _fakeCarService.GetByModel("Camry");
            Assert.IsNotNull(carsByModel);
            Assert.AreEqual(1, carsByModel.Count());
        }

        [Test]
        public void GetByColor_ReturnsCarsByColor()
        {
            IEnumerable<Car> carsByColor = _fakeCarService.GetByColor("Black");
            Assert.IsNotNull(carsByColor);
            Assert.AreEqual(1, carsByColor.Count());
        }

        [Test]
        public void DetailedSorting_ReturnsCarsByDetailedCriteria()
        {
            var sortingCriteria = new DetailedSorting
            {
                Mark = "Toyota",
                Model = "Camry",
                Color = "Black",
                minPower = 3.0,
                maxPower = 4.0,
                minPrice = 150,
                maxPrice = 200,
                minAge = 2020,
                maxAge = 2025
            };

            IEnumerable<Car> detailedSortingResult = _fakeCarService.DetailedSorting(sortingCriteria);
            Assert.IsNotNull(detailedSortingResult);
            Assert.AreEqual(1, detailedSortingResult.Count());
        }
        [Test]
        public void ChangeNotAvailableStatusToAvailable_ReturnsFalseForInvalidCarId()
        {
            bool statusChanged = _fakeCarService.ChangeNotAvailableStatusToAvailable(10);
            Assert.IsFalse(statusChanged);
        }
        [Test]
        public void ChangeAvailableStatusToNotAvailable_ReturnsTrueForValidCarId()
        {
            bool statusChanged = _fakeCarService.ChangeAvailableStatusToNotAvailable(1);
            Assert.IsTrue(statusChanged);
        }
        [Test]
        public void ChangeAvailableStatusToNotAvailable_ReturnsFalseForInvalidCarId()
        {
            bool statusChanged = _fakeCarService.ChangeAvailableStatusToNotAvailable(10);
            Assert.IsFalse(statusChanged);
        }
        [Test]
        public void DeleteCar_ReturnsTrueForValidCarId()
        {
            bool carDeleted = _fakeCarService.DeleteCar(1);
            Assert.IsTrue(carDeleted);
        }

        [Test]
        public void DeleteCar_ReturnsFalseForInvalidCarId()
        {
            bool carDeleted = _fakeCarService.DeleteCar(10);
            Assert.IsFalse(carDeleted);
        }
    }
}
