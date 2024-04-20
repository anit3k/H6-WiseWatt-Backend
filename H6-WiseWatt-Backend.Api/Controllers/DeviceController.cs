using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Api.Utils;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceManager _deviceManager;
        private readonly DeviceDTOMapper _deviceMapper;
        private readonly AuthenticationUtility _utility;

        public DeviceController(IDeviceManager deviceManager, DeviceDTOMapper deviceMapper, AuthenticationUtility utility)
        {
            _deviceManager = deviceManager;
            _deviceMapper = deviceMapper;
            _utility = utility;
        }

        #region Get Device State
        [HttpGet]
        [Route("api/device/state")]
        public async Task<IActionResult> GetState(string serial)
        {
            try
            {
                var result = await _deviceManager.GetDevice(serial);
                if (result == null)
                {
                    return BadRequest("No Device Found!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }
        #endregion

        #region Get All User Devices
        [HttpGet]
        [Route("api/device/devices")]
        [Authorize]
        public async Task<IActionResult> GetDevices()
        {
            try
            {
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var deviceEntities = await GetCurrentUserDevices(userGuid);
                return Ok(deviceEntities.Select(dm => _deviceMapper.MapToDeviceDto(dm)).ToList());
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
        [Route("api/device/updateDevice")]
        [Authorize]
        public async Task<IActionResult> UpdateDevice(DeviceDTO device)
        {
            try
            {
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }
                var deviceEntity = _deviceMapper.MapToDeviceEntity(device);

                await _deviceManager.UpdateDevice(deviceEntity);
                return Ok("Device Updated");
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }
        #endregion      
        private async Task<List<IoTDeviceBaseEntity>> GetCurrentUserDevices(string? userId)
        {
            Log.Information($"User {userId} request a list of all devices");
            var result = await _deviceManager.GetDevices(userId);
            return result;
        }        
    }
}
