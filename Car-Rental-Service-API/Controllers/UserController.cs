using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Helpers;
using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.Services;
using Car_Rental_Service_API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Car_Rental_Service_API.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly CarRentalContext _context;
        private readonly AppSettings _appSettings;
        public UserController(CarRentalContext context, IUserService userService, AppSettings appSettings)
        {
            _context = context;
            _service = userService;
            _appSettings = appSettings;
        }

        [HttpPost("Register")]
        public IActionResult RegisterNewUser(RegisterModel newUser)
        {
            var userValidation = new RegisterValidation(_context);
            var validationResults = userValidation.Validate(newUser);
            if (!validationResults.IsValid)
            {
                return BadRequest(validationResults.Errors);
            }
            else
            {
                _service.RegisterUser(newUser);
                _context.SaveChanges();
                return Ok(new { Message = $"User: {newUser.UserName} has successfully registered!" });
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var userLogin = _service.LoginUser(login);
            if (userLogin == null)
            {
                return BadRequest(new { message = "Username, Password or E-Mail is incorrect!" });
            }
            else
            {
                Loggs log = new Loggs
                {
                    UserLogged = $"User {userLogin.Email} with Username: {userLogin.UserName} logged in.",
                    LoggDate = DateTime.Now
                };
                _context.Loggs.Add(log);
                _context.SaveChanges();


                var tokenString = GenerateToken(userLogin);

                return Ok(new
                {
                    Message = "You have successfully Logged!",
                    UserName = userLogin.UserName,
                    Email = userLogin.Email,
                    Role = userLogin.Role,
                    Token = tokenString
                });
            }
        }

        [Authorize]
        [HttpGet("GetMyBalance")]
        public IActionResult GetMyBalance()
        {
            var userId = int.Parse(User.Identity.Name);
            var existingUser = _context.Users.FirstOrDefault(user => user.Id == userId);
            return Ok($"You have: {existingUser.Balance} GEL");
        }
        private object GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
