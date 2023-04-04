using System.Security.Cryptography;
using System.Text;

namespace WebLearning.Application.Ultities
{
    public class Password
    {
        public static string HashedPassword(string passWord)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(passWord);
            var hasedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hasedPassword);
        }
    }
}
