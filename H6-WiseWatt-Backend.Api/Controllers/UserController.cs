using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("api/user/register")]
        public async Task<IActionResult> RegisterUser(UserDTO user)
        {
            try
            {
                if (IsNotValid(user))
                {
                    return BadRequest("Invalid User Information");
                }

                if (await DoUserExist(user))
                {
                    return BadRequest("User already exist");
                }

                if (await AddNewUserToRepo(user))
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

        private bool IsNotValid(UserDTO user)
        {
            return user == null || string.IsNullOrWhiteSpace(user.Firstname) && string.IsNullOrWhiteSpace(user.Password) && string.IsNullOrWhiteSpace(user.Email);
        }

        private async Task<bool> DoUserExist(UserDTO user)
        {
            var result = await _userManager.ValidateUserByEmail(user.Email);
            return result;
        }

        private async Task<bool> AddNewUserToRepo(UserDTO user)
        {
            return await _userManager.CreateNewUser(new UserEntity { Password = user.Password, Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email });
        }
    }
}
