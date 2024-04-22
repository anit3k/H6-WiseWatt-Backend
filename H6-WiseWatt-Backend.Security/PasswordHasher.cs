using H6_WiseWatt_Backend.Security.Interfaces;
using System.Security.Cryptography;

namespace H6_WiseWatt_Backend.Security
{
    /// <summary>
    /// The PasswordHasher class provides methods to enhance password security by generating random salts and hashing passwords with those salts. 
    /// It ensures that passwords are stored securely in a way that mitigates common attacks such as brute force, dictionary, and rainbow table attacks. 
    /// The class uses the RandomNumberGenerator for salt generation and the SHA-256 hashing algorithm for password hashing.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        #region Public Methods
        /// <summary>
        /// This method creates a 16-byte random salt using a cryptographic random number generator (RandomNumberGenerator). 
        /// The resulting byte array is then converted to a Base64-encoded string and returned. 
        /// This salt is intended to be used for password hashing to improve security by reducing the risk of attacks like rainbow table attacks.
        /// </summary>
        /// <returns>string - The generated salt, encoded as a Base64 string.</returns>
        public string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        /// <summary>
        /// This method concatenates the provided password with the given salt, then hashes the concatenated string using the SHA-256 algorithm. 
        /// The resulting hash bytes are then converted to a Base64-encoded string and returned. 
        /// The salt helps to prevent certain types of attacks and ensures unique hashes for similar passwords.
        /// </summary>
        /// <param name="password">string password - The password to hash.</param>
        /// <param name="salt">string salt - The salt to add to the password before hashing.</param>
        /// <returns>string - The resulting hash, encoded as a Base64 string.</returns>
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
        #endregion
    }
}
