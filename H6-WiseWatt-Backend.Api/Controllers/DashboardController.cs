using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUserDeviceRepo _userDeviceRepo;

        public DashboardController(IUserDeviceRepo userDeviceRepo)
        {
            _userDeviceRepo = userDeviceRepo;
        }

        [HttpGet]
        [Route("api/dashboard/devices")]
        public async Task<IActionResult> GetDevices()
        {
            try
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (userEmail == null || string.IsNullOrEmpty(userEmail))
                {
                    return BadRequest("Invalid User Information");
                }

                var result = await _userDeviceRepo.GetDevices(userEmail);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");                
            }
        }
    }
}
