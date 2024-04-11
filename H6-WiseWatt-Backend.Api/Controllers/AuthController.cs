using H6_WiseWatt_Backend.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Invalid User Information");
            }
           
            return Ok("token");
        }
    }
}
