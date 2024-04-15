using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.Security.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IAuthService _authService;

        public LoginController(IUserRepo userRepo, IAuthService authService)
        {
            _userRepo = userRepo;
            _authService = authService;
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> Login(LoginDto user)
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

                var token = _authService.GenerateJSonWebToken(existingUser);
                Log.Information($"User {user.Email} has logged in");
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }       

        private bool IsNotValid(LoginDto user)
        {
            return user == null || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password);
        }

        private async Task<UserEntity> GetUserFromDb(LoginDto user)
        {
            return await _userRepo.GetUser(new UserEntity { Email = user.Email, Password = user.Password });
        }

        private bool IsUserNull(UserEntity existingUser)
        {
            return existingUser == null;
        }
    }
}
