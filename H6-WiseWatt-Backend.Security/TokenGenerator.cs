using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Security.Interfaces;
using H6_WiseWatt_Backend.Security.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace H6_WiseWatt_Backend.Security
{
    /// <summary>
    /// Responsible for creating JWTs that are used for user authentication and authorization in a secure system. 
    /// It interacts with configuration settings to obtain JWT-related parameters such as token secret, issuer, audience, and expiration time. 
    /// The class uses cryptographic methods to ensure the security of the generated tokens.
    /// </summary>
    public class TokenGenerator : ITokenGenerator
    {
        #region fields
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public TokenGenerator(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a JWT for a specified user by adding claims, configuring the token's security credentials, and setting the issuer, audience, and expiration time. 
        /// It uses the HMAC SHA-256 algorithm to sign the token and returns the token as a Base64-encoded string.
        /// </summary>
        /// <param name="user">UserEntity user: The user entity for which the token is generated.</param>
        /// <returns>string - The generated JWT as a string</returns>
        public string GenerateJSonWebToken(UserEntity user)
        {
            var model = GetJwtSettings();

            var claims = new[]
                {
                    new Claim("UserId", user.UserGuid),
                    new Claim(ClaimTypes.Email, user.Email),
                };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(model.TokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: model.TokenIssuer,
                audience: model.TokenAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(model.TokenExpirationMinutes),
                signingCredentials: creds
            );

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            return jwtTokenHandler.WriteToken(token);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Fetches the JWT-related configuration settings from a configuration source (e.g., appsettings.json). 
        /// It is used internally by the TokenGenerator to configure the generation of JSON Web Tokens.
        /// </summary>
        /// <returns>JwtSettingModel - The JWT settings model containing token secret, issuer, audience, and expiration information.</returns>
        private JwtSettingModel GetJwtSettings()
        {
            return _config.GetSection("JWT").Get<JwtSettingModel>();
        }
        #endregion
    }
}
