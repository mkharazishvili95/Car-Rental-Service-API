using Car_Rental_Service_API.Helpers;
using Car_Rental_Service_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Car_Rental_Service_API.Database
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext>options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Loggs> Loggs { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rent> Rents {  get; set; }
        public DbSet<CarRentalStatement> CarRentalStatement { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Age = 28,
                    IdentityNumber = "Unknown",
                    ContactNumber = "Unknown",
                    Balance = 0,
                    Email = "Admin123@gmail.com",
                    UserName = "Admin123",
                    Password = HashSettings.HashPassword("Admin123"),
                    Role = "Admin"
                },
                
                new User
                {
                    Id = 2,
                    FirstName = "User",
                    LastName = "User",
                    Age = 28,
                    IdentityNumber = "12345678910",
                    ContactNumber = "123456789",
                    Balance = 1000,
                    Email = "User123@gmail.com",
                    UserName = "User123",
                    Password = HashSettings.HashPassword("User123"),
                    Role = "User"
                });
            modelBuilder.Entity<Car>().HasData(
               new Car
               {
                   Id = 1,
                   Mark = "Mercedes-Benz",
                   Model = "E-200",
                   Color = "Grey",
                   Age = 2008,
                   EnginePower = 1.8,
                   Price = 80,
                   FuelType = "Gasoline",
                   IsAvailable = true
               });
            modelBuilder.Entity<Car>().HasData(
               new Car
               {
                   Id = 2,
                   Mark = "Toyota Camry",
                   Model = "SE",
                   Color = "Grey",
                   Age = 2020,
                   EnginePower = 2.5,
                   Price = 180,
                   FuelType = "Gas",
                   IsAvailable = true
               });
        }
    }
}
