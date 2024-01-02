using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Identity;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Services;
using Car_Rental_Service_API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Car_Rental_Service_API.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly CarRentalContext _context;
        public CarController(ICarService carService, CarRentalContext context)
        {
            _carService = carService;
            _context = context;
        }

        [HttpGet("GetAllCars")]
        public IActionResult GetAllCars()
        {
            var carList = _context.Cars.ToList();
            if(carList.Count == 0)
            {
                return NotFound(new { Message = "There is no any car at the moment!" });
            }
            else
            {
                return Ok(_carService.GetAllCars());
            }
        }

        [HttpGet("GetAvailableCars")]
        public IActionResult GetAvailableCars()
        {
            var availableCars = _context.Cars.Where(cars => cars.IsAvailable == true);
            if(availableCars == null)
            {
                return BadRequest(new { Message = "Cars are not available now!" });
            }
            else
            {
                return Ok(_carService.AvailableCars());
            }
        }

        [HttpGet("GetByMark")]
        public IActionResult GetByMark(string mark)
        {
            var getByMark = _context.Cars.Where(car => car.Mark.ToUpper().Contains(mark.ToUpper())).ToList();
            if(getByMark.Count == 0)
            {
                return NotFound(new { Message = $"There is no any {mark} car in the database!" });
            }
            else
            {
                return Ok(_carService.GetByMark(mark));
            }
        }

        [HttpGet("GetByModel")]
        public IActionResult GetByModel(string carModel)
        {
            var getByModel = _context.Cars.Where(car => car.Model.Contains(carModel.ToUpper())).ToList();
            if(getByModel == null || getByModel.Count == 0)
            {
                return NotFound(new { Message = $"There is no any {carModel} cars in the database!" });
            }
            else
            {
                return Ok(_carService.GetByModel(carModel));
            }
        }

        [HttpGet("GetByColor")]
        public IActionResult GetByColor(string carColor)
        {
            var getByColor = _context.Cars.Where(car => car.Color.ToUpper().Contains(carColor.ToUpper())).ToList();
            if(getByColor == null || getByColor.Count == 0)
            {
                return NotFound(new { Message = $"There is no any {carColor} cars in the database!" });
            }
            else
            {
                return Ok(_carService.GetByColor(carColor));
            }
        }

        [HttpGet("SortByPrice")]
        public IActionResult SortByPrice([FromBody] PriceRange priceRange)
        {
            double minPrice = priceRange.MinPrice;
            double maxPrice = priceRange.MaxPrice;

            var sortedCars = _context.Cars.Where(car => car.Price >= minPrice && car.Price <= maxPrice).ToList();
            if (sortedCars == null || sortedCars.Count == 0)
            {
                return NotFound(new { Message = $"There are no cars within the price range of {minPrice} to {maxPrice} GEL." });
            }
            else
            {
                return Ok(_carService.SortByPrice(minPrice, maxPrice));
            }
        }

        [HttpGet("SortByEnginePower")]
        public IActionResult SortByEnginePower([FromBody] EnginePowerRange enginePowerRange)
        {
            double minPower = enginePowerRange.minPower;
            double maxPower = enginePowerRange.maxPower;

            var sortedByEnginePower = _context.Cars.Where(car => car.EnginePower >= minPower && car.EnginePower <= maxPower).ToList();
            if (sortedByEnginePower == null || sortedByEnginePower.Count == 0)
            {
                return NotFound(new { Message = $"There are no cars within the engine power range of {minPower} to {maxPower}." });
            }
            else
            {
                return Ok(_carService.SortByEnginePower(minPower, maxPower));
            }
        }

        [HttpGet("DetailedSorting")]
        public IActionResult DetailedSorting(DetailedSorting sorting)
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

            if (detailedSorting == null || detailedSorting.Count == 0)
            {
                return NotFound(new { Message = "There is no any car by this Sort!" });
            }
            else
            {
                return Ok(_carService.DetailedSorting(sorting));
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("AddNewCar")]
        public IActionResult AddNewCar(Car newCar)
        {
            var carValidation = new CarValidation();
            var validationResults = carValidation.Validate(newCar);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }
            else
            {
                _carService.AddNewCar(newCar);
                return Ok(new { Message = $"Car: {newCar.Mark} - {newCar.Model} has successfully created!" });
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("UpdateCar")]
        public IActionResult UpdateCar(int carId, Car updateCar)
        {
            var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
            if(existingCar == null)
            {
                return BadRequest(new { Message = $"There is no any car by ID: {carId} to update!" });
            }
            else
            {
                var carValidation = new CarValidation();
                var validationResults = carValidation.Validate(updateCar);
                if (!validationResults.IsValid)
                {
                    return BadRequest(validationResults.Errors);
                }
                else
                {
                    _carService.UpdateCar(updateCar, carId);
                    return Ok(new { Message = "Car has successfully updated!" });
                }
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("ChangeNotAvailableStatusToAvailable")]
        public IActionResult ChangeNotAvailableStatusToAvailable(int carId)
        {
            var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
            if(existingCar == null)
            {
                return BadRequest(new { Message = $"There is no any Car in the database to change status!" });
            }
            if(existingCar.IsAvailable == true)
            {
                return BadRequest(new { Message = "That car is already Available!" });
            }
            if(existingCar.IsAvailable == false)
            {
                _carService.ChangeNotAvailableStatusToAvailable(carId);
                _context.SaveChanges();
            }
            return Ok(new { Message = $"{existingCar.Mark} - {existingCar.Model} Available status have changed!" });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("ChangeAvailableStatusToNotAvailable")]
        public IActionResult ChangeAvailableStatusToNotAvailable(int carId)
        {
            var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
            if(existingCar == null)
            {
                return BadRequest(new { Message = $"There is no any Car by ID: {carId} to change status!" });
            }
            if(existingCar.IsAvailable == false)
            {
                return BadRequest(new { Message = "That car is already not available!" });
            }
            if(existingCar.IsAvailable == true)
            {
                _carService.ChangeAvailableStatusToNotAvailable(carId);
                _context.SaveChanges();
            }
            return Ok(new { Message = $"{existingCar.Mark} - {existingCar.Model} Available status have changed!" });
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("DeleteCar")]
        public IActionResult DeleteCar(int carId)
        {
            var existingCar = _context.Cars.SingleOrDefault(car => car.Id == carId);
            if(existingCar == null)
            {
                return BadRequest(new { Message = $"There is no any car by ID: {carId} to delete!" });
            }
            else
            {
                _carService.DeleteCar(carId);
                _context.SaveChanges();
                return Ok(new { Message = "Car has successfully deleted from the database!" });
            }
        }
    }
}
