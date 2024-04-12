using H6_WiseWatt_Backend.MySqlData;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.AspNetCore.Mvc;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly MySqlDbContext _dbContext;

        public DevController(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// This method is used for development purpose, and is used to reset the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/dev/reset")]
        public async Task<IActionResult> ResetDb()
        {
            try
            {
                await _dbContext.Database.EnsureDeletedAsync();
                await _dbContext.Database.EnsureCreatedAsync();
                await AddDefaultUser();
                return Ok("Db has been reset");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        private async Task AddDefaultUser()
        {
            var user = new UserDbModel
            {
                Firstname = "Luke",
                Lastname = "Skywalker",
                Email = "luke@skywalker.com",
                Password = "Kode1234!"
            };
            _dbContext.Users.Add(user);

            var carCharger = new DeviceDbModel
            {
                DeviceName = "Car Charger",
                PowerConsumptionPerHour = 2.5
            };
            var heatPump = new DeviceDbModel
            {
                DeviceName = "Heat Pump",
                PowerConsumptionPerHour = 3.0
            };
            _dbContext.Devices.AddRange(new[] { carCharger, heatPump });

            await _dbContext.SaveChangesAsync();
        }
    }
}
