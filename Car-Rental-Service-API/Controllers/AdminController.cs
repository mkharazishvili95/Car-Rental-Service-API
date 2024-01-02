using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Identity;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_Service_API.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly CarRentalContext _context;
        public AdminController(IAdminService adminService, CarRentalContext context)
        {
            _adminService = adminService;
            _context = context;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var userList = _context.Users.ToList();
            if (userList.Count == 0)
            {
                return NotFound(new { Message = "There is no any User yet!" });
            }
            else
            {
                return Ok(_adminService.GetAllUsers());
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetById")]
        public IActionResult GetById(int userId)
        {
            var existingUser = _context.Users.SingleOrDefault(user => user.Id == userId);
            if (existingUser == null)
            {
                return NotFound(new { Message = $"There is no any User by ID: {userId}" });
            }
            else
            {
                var userDetails = new
                {
                    existingUser.FirstName,
                    existingUser.LastName,
                    existingUser.Age,
                    existingUser.ContactNumber,
                    existingUser.IdentityNumber,
                    existingUser.Email,
                    existingUser.UserName,
                    existingUser.Role
                };
                return Ok(userDetails);
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("RentCancelation")]
        public IActionResult RentCancelation(int rentId)
        {
            var existingRent = _context.Rents.SingleOrDefault(rent => rent.Id == rentId);
            if (existingRent == null)
            {
                return BadRequest(new { Message = $"There is no any rents by ID: {rentId}" });
            }

            var existingCar = _context.Cars.SingleOrDefault(car => car.Id == existingRent.carId);
            var existingUser = _context.Users.SingleOrDefault(user => user.Id == existingRent.userId);

            if (existingCar == null || existingUser == null)
            {
                return BadRequest(new {Message = "Car or User isn't exist!"});
            }
           
            else
            {
                var companyStatement = _context.CarRentalStatement.SingleOrDefault() ?? new CarRentalStatement();
                TimeSpan rentalDuration = existingRent.RentTo - existingRent.RentFrom;
                int rentalDurationInDays = (int)rentalDuration.TotalDays;
                double rentalCost = existingCar.Price * rentalDurationInDays;
                if(companyStatement.Balance < rentalCost)
                {
                    return BadRequest(new { Message = "BadRequest - Contact to our support!" });
                }
                existingUser.Balance += rentalCost;
                companyStatement.Balance -= rentalCost;
                existingCar.IsAvailable = true;
                _context.Update(existingCar);
                _context.Update(companyStatement);
                _context.Update(existingUser);
                _context.Remove(existingRent);
                _context.SaveChanges();
                _context.SaveChanges();
                return Ok(new { Message = "Rent has successfully canceled!" });
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("MakeARefund")]
        public IActionResult MakeARefund(MakeARefund refundModel)
        {
            var existingUser = _context.Users.SingleOrDefault(user => user.Id == refundModel.userId);
            if (existingUser == null)
            {
                return BadRequest(new { Message = $"There is no any User by ID: {refundModel.userId}!" });
            }
            var companyStatement = _context.CarRentalStatement.SingleOrDefault() ?? new CarRentalStatement();
            if (companyStatement.Balance < refundModel.Amount)
            {
                return BadRequest(new { Message = $"You can not transfer money to {existingUser.UserName}, because account statement is: {companyStatement.Balance} and sending amount is: {refundModel.Amount}" });
            }
            else
            {
                _adminService.MakeRefund(refundModel);
                return Ok(new { Message = $"You have transfered {refundModel.Amount} GEL to {existingUser.UserName}" });
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("CancelRentWithoutRefund")]
        public IActionResult CancelRentWithoutRefunt(int rentId)
        {
            var existingRent = _context.Rents.SingleOrDefault(rent => rent.Id == rentId);
            if (existingRent == null)
            {
                return BadRequest(new { Message = $"There is no any rent by ID: {rentId} to cancel!" });
            }
            var existingCar = _context.Cars.FirstOrDefault(car => car.Id == existingRent.carId);
            if (existingCar == null)
            {
                return BadRequest(new { Message = $"There is no any cars by this Id!" });
            }
            else
            {
                _adminService.CancelRentWithoutRefund(rentId);
                return Ok(new { Message = "Rent has canceled without any refund!" });
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAllRents")]
        public IActionResult GetAllRents()
        {
            var rentList = _context.Rents.ToList();
            if(rentList.Count == 0)
            {
                return NotFound(new { Message = "There is no any rent yet!" });
            }
            else
            {
                return Ok(_adminService.GetAllRents());
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetRentById")]
        public IActionResult GetRentById(int rentId)
        {
            var getRentById = _context.Rents.SingleOrDefault(rent => rent.Id == rentId);
            if(getRentById == null)
            {
                return NotFound(new { Message = $"There is no any rent by ID: {rentId}" });
            }
            else
            {
                return Ok(_adminService.GetRentById);
            }
        }
    }
}
