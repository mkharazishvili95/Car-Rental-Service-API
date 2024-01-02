using Car_Rental_Service_API.Models;
using FluentValidation;

namespace Car_Rental_Service_API.Validation
{
    public class CarValidation : AbstractValidator<Car>
    {
        public CarValidation() 
        {
            RuleFor(car => car.Mark).NotEmpty().WithMessage("Enter Car Mark!");
            RuleFor(car => car.Model).NotEmpty().WithMessage("Enter Car Model!");
            RuleFor(car => car.Age).NotEmpty().WithMessage("Enter Car Age!")
                .GreaterThanOrEqualTo(1900).LessThanOrEqualTo(2025).WithMessage("Enter Car Age!");
            RuleFor(car => car.Color).NotEmpty().Length(3, 20).WithMessage("Enter Car Color!");
            RuleFor(car => car.EnginePower).NotEmpty().WithMessage("Enter Engine Power of the Car!");
            RuleFor(car => car.Price).NotEmpty().WithMessage("Enter Car Price!");
            RuleFor(car => car.FuelType).NotEmpty().WithMessage("Enter Fuel Type!")
            .Must(fuelType => fuelType == "Gasoline" || fuelType == "Diesel" || fuelType == "Electrical" || fuelType == "Gas")
            .WithMessage("Enter one of these options: Gasoline, Diesel, Electrical, or Gas!");
        }
    }
}
