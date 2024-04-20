using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public UnitController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        #region GetAllUsersDevices
        [HttpGet]
        [Route("api/device/state")]
        public async Task<IActionResult> GetState(string serial)
        {
            try
            {
                var result = await _deviceService.GetDevice(serial);
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
    }
}
