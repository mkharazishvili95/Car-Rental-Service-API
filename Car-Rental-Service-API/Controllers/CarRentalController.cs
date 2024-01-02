using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Service_API.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class CarRentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly CarRentalContext _context;
        public CarRentalController(IRentalService rentalService, CarRentalContext context)
        {
            _rentalService = rentalService;
            _context = context;
        }

        [Authorize]
        [HttpPost("RentACar")]
        public IActionResult RentACar(RentRequest rentRequest)
        {
            Rent rentModel = new Rent();
            var userId = int.Parse(User.Identity.Name);
            var existingUser = _context.Users.SingleOrDefault(user => user.Id == userId);
            var existingCar = _context.Cars.SingleOrDefault(car => car.Id == rentRequest.carId);
            var companyStatement = _context.CarRentalStatement.SingleOrDefault() ?? new CarRentalStatement();

            if (existingUser == null)
            {
                return BadRequest("Bad Request!");
            }
            if (existingCar == null)
            {
                return BadRequest(new { Message = $"There is no any car by ID: {rentRequest.carId}" });
            }
            if (existingCar.IsAvailable == false)
            {
                return BadRequest(new { Message = "That car is not available at the moment!" });
            }
            var priceCalculator = existingUser.Balance >= (existingCar.Price * rentRequest.rentDays);
            if (!priceCalculator)
            {
                return BadRequest(new { Message = $"You have not anough money to rent that car. Your balance is: {existingUser.Balance} GEL, and that car rent costs: {existingCar.Price * rentRequest.rentDays} GEL for {rentRequest.rentDays} days!" });
            }
            else
            {
                rentModel.userId = userId;
                rentModel.carId = rentRequest.carId;
                rentModel.RentFrom = DateTime.Now;
                rentModel.RentTo = DateTime.Now.AddDays(rentRequest.rentDays);
                _context.Add(rentModel);
                existingCar.IsAvailable = false;
                existingUser.Balance -= (existingCar.Price * rentRequest.rentDays);
                companyStatement.Balance += (existingCar.Price * rentRequest.rentDays);
                _context.Update(existingCar);
                _context.Update(existingUser);
                _context.Update(companyStatement);
                _context.SaveChanges();

            }
            return Ok(new { Message = $"You have rented: {existingCar.Mark} - {existingCar.Model} successfully!" });
        }

        [Authorize]
        [HttpPost("AddRentalDays")]
        public IActionResult AddRentalDays(DaysAddToRental rentalAdd)
        {
            var userid = int.Parse(User.Identity.Name);
            var existingUser = _context.Users.SingleOrDefault(user => user.Id == userid);
            if (existingUser == null)
            {
                return BadRequest();
            }
            var existingRent = _context.Rents.SingleOrDefault(rent => rent.Id == rentalAdd.rentId);
            if (existingRent == null)
            {
                return BadRequest(new { Message = $"There is no any rent by ID: {rentalAdd.rentId}" });
            }
            if (existingRent.userId != existingUser.Id)
            {
                return BadRequest(new { Message = "That is not your rented car!" });
            }

            var existingCar = _context.Cars.SingleOrDefault(car => car.Id == existingRent.carId);
            var priceCalculator = (existingCar.Price * rentalAdd.rentalDays);
            var companyStatement = _context.CarRentalStatement.SingleOrDefault() ?? new CarRentalStatement();
            if (existingUser.Balance < priceCalculator)
            {
                return BadRequest(new { Message = $"Your balance is: {existingUser.Balance} GEL and that car rent costs: {priceCalculator} GEL for {rentalAdd.rentalDays} days!" });
            }

            if (existingUser.Balance >= (priceCalculator * rentalAdd.rentalDays))
            {
                existingRent.RentTo = existingRent.RentTo.AddDays(rentalAdd.rentalDays);
                existingUser.Balance -= priceCalculator;
                companyStatement.Balance += priceCalculator;
                _context.Update(existingRent);
                _context.Update(existingUser);
                _context.Update(companyStatement);
                _context.SaveChanges();

                return Ok(new { Message = $"You have added {rentalAdd.rentalDays} to your rent and have paid {priceCalculator} GEL for it!" });
            }
            else
            {
                return BadRequest(new { Message = "Insufficient balance to add rental days." });
            }
        }
    }
}

