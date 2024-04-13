using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
                var userEmail = GetEmailFromClaim();
                if (ValidateEmail(userEmail))
                {
                    return BadRequest("Invalid User");
                }

                return Ok(await GetCurrentUserDevices(userEmail));
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }

        private string? GetEmailFromClaim()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }

        private static bool ValidateEmail(string? userEmail)
        {
            return userEmail == null || string.IsNullOrEmpty(userEmail);
        }

        private async Task<List<DeviceEntity>> GetCurrentUserDevices(string? userEmail)
        {
            Log.Information($"User {userEmail} is getting all devices");
            return await _userDeviceRepo.GetDevices(userEmail);
        }
    }
}
