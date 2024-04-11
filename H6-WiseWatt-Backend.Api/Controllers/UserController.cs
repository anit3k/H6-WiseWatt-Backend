using H6_WiseWatt_Backend.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("api/user/register")]
        public async Task<IActionResult> RegisterUser(UserDto user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) && string.IsNullOrWhiteSpace(user.Password) && string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest("Invalid User Information");
            }

            if (await DoUserExist(user))
            {
                return BadRequest("User name or Email does already exist");
            }

            return Ok("User has been created");
        }

        private async Task<bool> DoUserExist(UserDto user)
        {            
            return true;
        }
    }
}
