using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.Security.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ITokenGenerator _tokenGen;

        public AuthenticationController(IUserManager userManager, ITokenGenerator tokenGen)
        {
            _userManager = userManager;
            _tokenGen = tokenGen;
        }

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
                UserEntity existingUser = await GetUserFromDb(user);

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

        private bool IsNotValid(LoginDTO user)
        {
            return user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password);
        }

        private async Task<UserEntity> GetUserFromDb(LoginDTO user)
        {
            return await _userManager.GetUser(new UserEntity { Email = user.Email });
        }

        private bool IsUserNull(UserEntity existingUser)
        {
            return existingUser == null;
        }
    }
}
