using System.Text;
using Tweetinvi.Security;

namespace Car_Rental_Service_API.Helpers
{
    public class HashSettings
    {
        public static string HashPassword(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] passBytes = Encoding.ASCII.GetBytes(password);
            byte[] encryptBytes = sha1.ComputeHash(passBytes);
            return Convert.ToBase64String(encryptBytes);
        }
    }
}
