using iRead.API.Utilities.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace iRead.API.Utilities
{
    public class EncryptionUtilities : IEncryptionUtilities
    {
        public string EncryptPassword(string password)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var byteValue = Encoding.UTF8.GetBytes(password);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}
