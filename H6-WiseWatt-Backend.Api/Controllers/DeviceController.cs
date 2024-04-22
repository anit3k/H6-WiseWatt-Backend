using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Api.Utils;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    /// <summary>
    /// Designed to manage IoT devices. It provides endpoints for retrieving device state, listing user-specific devices, 
    /// and updating device information. The class relies on multiple services and utility classes to manage device-related operations, 
    /// ensure user authentication, and map data between domain entities and Data Transfer Objects (DTOs).
    /// </summary>
    [ApiController]
    public class DeviceController : ControllerBase
    {
        #region private fields
        private readonly IDeviceManager _deviceManager;
        private readonly DeviceDTOMapper _deviceMapper;
        private readonly AuthenticationUtility _utility;
        #endregion

        #region Constructor
        public DeviceController(IDeviceManager deviceManager, DeviceDTOMapper deviceMapper, AuthenticationUtility utility)
        {
            _deviceManager = deviceManager;
            _deviceMapper = deviceMapper;
            _utility = utility;
        }
        #endregion

        #region Get Device State
        /// <summary>
        /// A GET endpoint to retrieve the state of a device based on its serial number. 
        /// If the device is not found, it returns a 400 Bad Request with an appropriate message. 
        /// Otherwise, it returns the device information in an HTTP 200 OK response.
        /// </summary>
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
        /// <summary>
        ///  A GET endpoint that retrieves all devices associated with the authenticated user. It uses the AuthenticationUtility to get the user GUID from the token and validates it. 
        ///  If the user is invalid, it returns a 400 Bad Request; otherwise, it fetches and returns the devices in an HTTP 200 OK response.
        /// </summary>
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
        /// <summary>
        /// A POST endpoint to update a device's information. 
        /// It uses the DeviceDTOMapper to convert the DeviceDTO to a domain entity and updates it through IDeviceManager. 
        /// It returns a confirmation message upon success or handles errors appropriately.
        /// </summary>
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

        #region Private Methods
        /// <summary>
        /// Fetches all devices for a specific user and logs the request. It is used by other endpoints to retrieve user-specific device information.
        /// </summary>
        /// <param name="userId">Current User Id</param>
        /// <returns>Current user IoT devices</returns>
        private async Task<List<IoTDeviceBaseEntity>> GetCurrentUserDevices(string? userId)
        {
            Log.Information($"User {userId} request a list of all devices");
            var result = await _deviceManager.GetDevices(userId);
            return result;
        }        
        #endregion
    }
}
