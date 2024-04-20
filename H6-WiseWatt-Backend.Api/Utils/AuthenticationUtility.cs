using System.Security.Claims;

namespace H6_WiseWatt_Backend.Api.Utils
{
    public class AuthenticationUtility
    {
        public string? GetUserGuidFromToken(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        }

        public bool ValidateUser(string? userId)
        {
            return userId == null || string.IsNullOrEmpty(userId);
        }
    }
}
