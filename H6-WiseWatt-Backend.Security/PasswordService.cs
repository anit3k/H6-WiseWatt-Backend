using H6_WiseWatt_Backend.Security.Interfaces;
using System.Security.Cryptography;

namespace H6_WiseWatt_Backend.Security
{
    public class PasswordService : IPasswordService
    {
        public string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string HashPasswordWithSalt(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = String.Concat(password, salt);
                byte[] saltedPasswordBytes = System.Text.Encoding.UTF8.GetBytes(saltedPassword);
                byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
