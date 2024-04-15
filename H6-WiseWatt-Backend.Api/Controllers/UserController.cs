using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost]
        [Route("api/user/register")]
        public async Task<IActionResult> RegisterUser(UserDto user)
        {
            try
            {
                if (user == null || string.IsNullOrWhiteSpace(user.Firstname) && string.IsNullOrWhiteSpace(user.Password) && string.IsNullOrWhiteSpace(user.Email))
                {
                    return BadRequest("Invalid User Information");
                }

                if (await DoUserExist(user))
                {
                    return BadRequest("User already exist");
                }

                var result = await _userRepo.CreateNewUser(new UserEntity { Password = user.Password, Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email, UserGuid = "669cadd0-70cf-43a1-9a9d-426212185666" });

                if (result)
                {
                    return Ok("User has been created");
                }
                else
                {
                    return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }

        private async Task<bool> DoUserExist(UserDto user)
        {
            var result = await _userRepo.ValidateUserEmail(new UserEntity { Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email });
            return result;
        }
    }
}
