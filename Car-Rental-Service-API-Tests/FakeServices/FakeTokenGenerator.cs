using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.TokenGenerator;

namespace FakeServices
{
    public class FakeTokenGenerator : IAccessTokenGenerator
    {
        public string GenerateToken(User user)
        {
            string fakeToken = "Fake token =)";
            return fakeToken;
        }
    }
}
