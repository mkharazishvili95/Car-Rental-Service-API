using Car_Rental_Service_API.Models;
using Car_Rental_Service_API.TokenGenerator;
using NUnit.Framework;

namespace FakeServices
{
    [TestFixture]
    public class FakeTokenGeneratorTests
    {
        private IAccessTokenGenerator _fakeTokenGenerator;

        [SetUp]
        public void Setup()
        {
            _fakeTokenGenerator = new FakeTokenGenerator();
        }

        [Test]
        public void GenerateToken_ReturnsFakeToken()
        {
            var user = new User
            {
                Id = 1,
                UserName = "misho123",
                Role = "User"
            };
            var generatedToken = _fakeTokenGenerator.GenerateToken(user);
            Assert.IsNotNull(generatedToken);
        }
    }
}
