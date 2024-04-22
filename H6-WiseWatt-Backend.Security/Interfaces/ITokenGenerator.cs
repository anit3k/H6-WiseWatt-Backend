using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Security.Interfaces
{
    /// <summary>
    /// The ITokenGenerator interface specifies a method for creating JWTs based on user information. 
    /// Implementations of this interface generate JWTs that contain user-specific claims and security credentials to support secure authentication and authorization in the system.
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// creates a JWT based on user information, typically including user-specific claims like user ID and email. 
        /// The generated token can be used for authentication and authorization in a secure environment. 
        /// The implementation of this method will involve cryptographic signing to ensure the JWT's integrity and authenticity.
        /// </summary>
        /// <param name="user">UserEntity user: The user entity for which the JWT is generated.</param>
        /// <returns>string - The generated JWT as a Base64-encoded string.</returns>
        string GenerateJSonWebToken(UserEntity user);
    }
}
