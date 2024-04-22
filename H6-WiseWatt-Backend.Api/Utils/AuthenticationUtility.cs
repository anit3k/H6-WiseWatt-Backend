using System.Security.Claims;

namespace H6_WiseWatt_Backend.Api.Utils
{
    /// <summary>
    /// utility designed to assist with authentication-related tasks. 
    /// This class contains methods that work with user authentication tokens, 
    /// and provide basic validation and data extraction functionality.
    /// </summary>
    public class AuthenticationUtility
    {
        #region Internal Methods
        /// <summary>
        /// Used to retrieve a user's GUID from an authentication token where user-specific data is needed, 
        /// such as validating access to a protected resource or identifying the current user in a multi-user environment.        ///
        /// </summary>
        /// <param name="user">Current User Claim</param>
        /// <returns>Users unique identifier</returns>
        internal string? GetUserGuidFromToken(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        }

        /// <summary>
        /// The ValidateUser method is useful for quickly checking if a user identifier is valid or non-empty, 
        /// which is required for security checks and authorization logic.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>true/false</returns>
        internal bool ValidateUser(string? userId)
        {
            return userId == null || string.IsNullOrEmpty(userId);
        }
        #endregion
    }
}
