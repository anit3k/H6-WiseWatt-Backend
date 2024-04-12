namespace H6_WiseWatt_Backend.Security.Models
{
    public class JwtSettingEntity
    {
        public string TokenSecret { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public double TokenExpirationMinutes { get; set; }
    }
}
