namespace H6_WiseWatt_Backend.Security.Interfaces
{
    /// <summary>
    /// The IPasswordHasher interface provides a contract for implementing password security. 
    /// It specifies methods for generating a secure cryptographic salt and hashing a password using that salt with a secure algorithm. 
    /// This interface allows for flexibility in implementing various hashing strategies while maintaining a consistent contract for password security functions.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Generates a secure random salt, typically used for hashing passwords. 
        /// This salt is used to increase the security of hashed passwords and prevent attacks that rely on common patterns or precomputed tables.
        /// </summary>
        /// <returns>string - A Base64-encoded cryptographic salt.</returns>
        string GenerateSalt();

        /// <summary>
        /// Hashes a given password with a provided salt, typically using a cryptographic hashing algorithm like SHA-256. 
        /// This method ensures that passwords are securely stored in a way that is resistant to brute force and other types of attacks.
        /// </summary>
        /// <param name="password">string password - The password to hash.</param>
        /// <param name="salt">string salt - The salt to add to the password before hashing.</param>
        /// <returns>string - The hashed password, encoded as a Base64 string.</returns>
        string HashPasswordWithSalt(string password, string salt);
    }
}
