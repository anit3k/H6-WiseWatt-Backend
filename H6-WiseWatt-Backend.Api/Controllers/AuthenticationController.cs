using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.Security.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    /// <summary>
    /// Responsible for handling user authentication and login functionality within a web API context. 
    /// It uses interfaces to interact with user management and token generation services, providing a secure and flexible login endpoint.
    /// </summary>
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region private fields
        private readonly IUserManager _userManager;
        private readonly ITokenGenerator _tokenGen;
        #endregion

        #region Constructor
        public AuthenticationController(IUserManager userManager, ITokenGenerator tokenGen)
        {
            _userManager = userManager;
            _tokenGen = tokenGen;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// A POST endpoint that handles user login. It accepts a LoginDTO containing user credentials and performs the following tasks:
        /// Validates the provided user data to ensure it's not empty or invalid.
        /// Fetches the user from the database using the provided email.
        /// Verifies if the user exists and if the password is correct.
        /// Generates a JWT for the authenticated user if the credentials are valid.
        /// Logs the login event and returns an HTTP 200 response with the token if successful.
        /// Handles errors and returns appropriate HTTP responses for various failure cases, such as invalid credentials or server errors.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            try
            {
                if (IsNotValid(user))
                {
                    return BadRequest("Invalid User Information");
                }
                UserEntity existingUser = await GetUserFromManager(user);

                if (IsUserNull(existingUser))
                {
                    return BadRequest("Wrong user name or password");
                }

                var token = _tokenGen.GenerateJSonWebToken(existingUser);
                Log.Information($"User {user.Email} has logged in");
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Validates the input LoginDTO to ensure it's not null and contains valid email and password fields.
        /// </summary>
        /// <param name="user">Current User</param>
        /// <returns>true/false</returns>
        private bool IsNotValid(LoginDTO user)
        {
            return user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password);
        }

        /// <summary>
        /// Fetches a UserEntity from the user manager using the provided email.
        /// </summary>
        /// <param name="user">Current User</param>
        /// <returns></returns>
        private async Task<UserEntity> GetUserFromManager(LoginDTO user)
        {
            return await _userManager.GetUser(new UserEntity { Email = user.Email });
        }

        private bool IsUserNull(UserEntity existingUser)
        {
            return existingUser == null;
        }
        #endregion
    }
}
