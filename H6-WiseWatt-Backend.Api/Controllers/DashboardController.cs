using H6_WiseWatt_Backend.Api.Models;
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

        #region GetAllUsersDevices
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
        #endregion

        #region Update Device

        [HttpPost]
        [Route("api/dashboard/updateDevice")]
        public async Task<IActionResult> UpdateDevice(DeviceDto device)
        {
            try
            {
                var userEmail = GetEmailFromClaim();
                if (ValidateEmail(userEmail))
                {
                    return BadRequest("Invalid User");
                }

                await _userDeviceRepo.UpdateDevice(new DeviceEntity {DeviceName = device.DeviceName, PowerConsumptionPerHour = device.PowerConsumptionPerHour, SerialNumber = device.SerialNumber, IsOn = device.IsOn});

                return Ok(await GetCurrentUserDevices(userEmail));
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }
        #endregion


        private string? GetEmailFromClaim()
        {
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        }

        private bool ValidateEmail(string? userEmail)
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
