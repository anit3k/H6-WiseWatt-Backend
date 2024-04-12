using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            if (user == null || string.IsNullOrWhiteSpace(user.Firstname) && string.IsNullOrWhiteSpace(user.Password) && string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest("Invalid User Information");
            }

            if (await DoUserExist(user))
            {
                return BadRequest("User name or Email does already exist");
            }

            var result = await _userRepo.CreateNewUser(new UserEntity { Password = user.Password, Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email });

            if (result)
            {
                return Ok("User has been created");
            }
            else
            {
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }

        private async Task<bool> DoUserExist(UserDto user)
        {
            var result = await _userRepo.ValidateUsernameEmail(new UserEntity { Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email });
            return result;
        }
    }
}
