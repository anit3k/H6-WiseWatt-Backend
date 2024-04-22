namespace H6_WiseWatt_Backend.Security.Models
{
    /// <summary>
    /// Contains configuration properties for JWT token handling, which are used to secure user sessions and manage token-based authentication. 
    /// These settings include a secret key for token generation, the token issuer, the intended audience, and the token's expiration time in minutes.
    /// </summary>
    public class JwtSettingModel
    {
        public string TokenSecret { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public double TokenExpirationMinutes { get; set; }
    }
}
