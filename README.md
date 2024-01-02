# Car-Rental-Service-API
This is my WEB API project (on .Net 8) where users can register, login and rent cars daily.
## Details
After registering and entering the system, the user is assigned a validation token,
which allows it to perform the following operations: Get all car list (which is added by admin), sort cars by Mark,Model,Color,Age,EnginePower,Fuel type. And Users
can also detailed sorting. They can rent a car daily, if car is Available and user has anough money for it. The user also has the option to add the number of rented days.
2 Users were automatically generated after performing the migrations in the API. One with the role of admin and the other with the role of User.
And the administrator (who exists in the database as Admin123, Admin123@gmail.com) has the right to receive information about all users.
He can add new car in the database, update existing cars, cancel rental service and when he cancels it, user authomaticly gets refund from company statement to his balance.

### User Register Form:
/api/controller/Register
{
"firstName": "string",
  "lastName": "string",
  "age": 0,
  "identityNumber": "string",
  "contactNumber": "string",
  "email": "string",
  "userName": "string",
  "password": "string"
}

### User Login Form:
/api/controller/Login
{
   "email" : "string",
  "username": "string",
  "password": "string"
}
/api/controller/GetMyBalance     <= (User can know his own balance)

### Car:
/api/controller/GetAllCars  <=(Get detailed information, about all cars, that are added by Admin)
/api/controller/GetAvailableCars <= (Get information just for available cars)
/api/controller/GetByMark <= (Get information just for specific mark cars By adding: ?mark="?")
/api/controller/GetByModel <= (Get information just for specific model cars By adding: ?carModel="?")
/api/controller/GetByColor <= (Get information just for specific color cars By adding: ?carColor="?")
/api/controller/SortByPrice <= (User can sort cars by min and max prices):
{
  "minPrice": (double),
  "maxPrice": (double)
}
/api/controller/SortByEnginePower <= (User can sort cars by min and max engine powers):
{
  "minPower": (double),
  "maxPower": (double)
}
/api/controller/DetailedSorting <= (User can detailed sort cars):
{
  "mark": "string",
  "model": "string",
  "color": "string",
  "minAge": (int),
  "maxAge": (int),
  "minPrice": (double),
  "maxPrice": (double),
  "minPower": (double),
  "maxPower": (double)
}

/api/controller/AddNewCar <= (Admin can add new car):
{
  "mark": "string",
  "model": "string",
  "color": "string",
  "age": (int),
  "enginePower": (double),
  "fuelType": "string",
  "price": (double),
  "isAvailable: (bool)
  }
  
  /api/controller/UpdateCar?carId="id" <= (Admin can update existing car):
{
  "mark": "string",
  "model": "string",
  "color": "string",
  "age": (int),
  "enginePower": (double),
  "fuelType": "string",
  "price": (double),
  "isAvailable": (bool)
}
/api/controller/ChangeNotAvailableStatusToAvailable?carId="id"  <= (Admin can update not available status to available)
/api/controller/ChangeAvailableStatusToNotAvailable?carId="id"  <= (Admin can update available status to not available)
/api/controller/DeleteCar?carId="id"  <= (Admin can delete car from the database)
/api/controller/RentACar <= (User can rent a car, by car id and rent days):
{
  "carId": (int),
  "rentDays": (int)
}
/api/controller/AddRentalDays <= (The user can extend the rental days of the rented car):
{
  "rentId": (int),
  "rentDays": (int)
}
/api/controller/GetAllUsers <= (Admin can get information about all users)
/api/controller/GetById?userId="id"  <= (Admin can get information about user by user id)
/api/controller/RentCancelation?rentId="id"  <= (Admin can cancel rent by rentId and when he made it, user gets refund for it authomaticly)
/api/controller/MakeARefund  <= (Admin can refund user money(transfer from company balance to user's balance), without any rent canceling):
{
  "userId": (int),
  "amount": (double)
}
/api/controller/CancelWithoutRefund?rentId="id" <= (Admin can cancel rent without any refund)
/api/controller/GetAllRents <= (Admin can get list for rents)
/api/controller/GetRentById?rentId="id" <= (Admin can get specific rent by rent id)


## What I have made:
I created models: User, Car, CarRentalStatement, Rent, Loggs.
I created 2 Role-based authorization. (I used the following Nuget packages: Microsoft.AspNetCore.Identity.EntityFrameworkCore, Microsoft.AspNetCore.Authentication.JwtBearer) I have Admin and User in the project. To receive services, a person must first register. I created a user registration model and a login model. I have made validations for the registration model (eg the user's age must be 21 or older and the UserName and E-mail must not be identical to the UserName and E-Mail of the user in the database). For validations I used: FluentValidation. After user registration and logging in, log information is stored in the database (Select * from dbo.Loggs - outputs UserName, role and log-in date of the logged-in user), for this I created a Logs model.
After logging in, the user is assigned a token that is generated (I specified a conditional time of 365 days - 1 year)
And after writing the secret key of this token to JWT.IO, this token becomes valid, which allows the user to perform various operations and receive services.
I have created the already mentioned Car model, and I also made validations for it. Only admin can add cars. I also created services: AdminService, UserService, CarService, CarRentalService. I wrote business logic in it. Users who are already logged in can see their balance, View the list of added cars and rent any, that are available. After rent a car, the amount on his balance will be transferred to the company statement account that I have created. He can also see his rents.
The admin has the right to get information about all users, get information about specific users by Id.
It can also add a new car or modify an existing one. Also delete the car. He can also cancel any user rent. (After cancellation, the user will automatically get back the amount paid for this rent, which will be transferred from the company balance to user's balance)
I have used ORM: EntityFrameWorkCore. I created a Context file with the name: CarRentalContext, where I included the models in the project that I wanted to transfer to the database. It also creates an admin and user in the database after adding migrations.
I connected the project with the database (SQL that I created under the name of CarRentalSQL).
For this I used Nuget Packages: Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools.(Microsoft SQL Server Management Studio)
The passwords entered in the database are in a hashed state, for this I used: TweetinviAPI.
I tested the project with Postman to output the status codes I expected, And everything in the database is going as I expect it to go and everything works fine.
I also created a NUnit project where I created FakeServices where I included the existing services in the project. I wrote the tests and all 25 tests ran successfully.
Everything works perfectly.
