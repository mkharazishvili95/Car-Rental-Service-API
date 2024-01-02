using Car_Rental_Service_API.Database;
using Car_Rental_Service_API.Models;
using FluentValidation;

namespace Car_Rental_Service_API.Validation
{
    public class RegisterValidation : AbstractValidator<RegisterModel>
    {
        private readonly CarRentalContext _context;
        public RegisterValidation(CarRentalContext context) 
        {
            _context = context;

            RuleFor(newUser => newUser.FirstName).NotEmpty().WithMessage("Enter your FirstName!")
                .Must(firstName => !string.IsNullOrWhiteSpace(firstName))
                .WithMessage("Firstname must not contin Whitespace!");

            RuleFor(newUser => newUser.LastName).NotEmpty().WithMessage("Enter your LastName!")
                .Must(lastName => !string.IsNullOrWhiteSpace(lastName))
                .WithMessage("Lastname must not contin Whitespace!");

            RuleFor(newUser => newUser.Age).NotEmpty().WithMessage("Enter your Age!")
                .GreaterThanOrEqualTo(21).WithMessage("Your age must be 21 or more to register!");

            RuleFor(newUser => newUser.IdentityNumber).NotEmpty().WithMessage("Enter your Identity Number!")
                .Length(10,100).WithMessage("Identity # must be from 10 numbers");

            RuleFor(newUser => newUser.ContactNumber).NotEmpty().WithMessage("Enter your Contact Number(Mobile)")
                .Length(9, 9).WithMessage("Enter your Contact Number(Mobile)");

            RuleFor(newUser => newUser.Email).NotEmpty().WithMessage("Enter your E-Mail address!")
                .Must(DifferentEmail).WithMessage("E-Mail already exists. Try another!")
                .EmailAddress().WithMessage("Enter your valid E-Mail address!");

            RuleFor(newUser => newUser.UserName).NotEmpty().WithMessage("Enter your UserName!")
                .Length(6, 15).WithMessage("Username length must be from 6 to 15 chars or numbers!")
                .Must(DifferentUserName).WithMessage("Username already exists. Try another!");

            RuleFor(newUser => newUser.Password).NotNull().NotEmpty().WithMessage("Enter your Password!")
                .Length(6, 15).WithMessage("Password length must be from 6 to 15 chars or numbers!");
        }
        private bool DifferentEmail(string eMail)
        {
            try
            {
                var differentEmailAddress = _context.Users.SingleOrDefault(user => user.Email.ToUpper() == eMail.ToUpper());
                return differentEmailAddress == null;
            }
            catch
            {
                return false;
            }
        }
        private bool DifferentUserName(string userName)
        {
            try
            {
                var differentUserName = _context.Users.SingleOrDefault(user => user.UserName.ToUpper() == userName.ToUpper());
                return differentUserName == null;
            }
            catch
            {
                return false;
            }
        }
    }
}
