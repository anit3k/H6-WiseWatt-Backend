using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class UnitController : ControllerBase
    {
        public UnitController()
        {
            
        }
        #region GetAllUsersDevices
        [HttpGet]
        [Route("api/device/state")]
        public async Task<IActionResult> GetState()
        {
            try
            {
               
                return Ok("test");
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
