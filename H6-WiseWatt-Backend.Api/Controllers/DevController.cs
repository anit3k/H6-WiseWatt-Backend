﻿using H6_WiseWatt_Backend.MySqlData;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
                _dbContext.ChangeTracker.Clear();
                await AddDefaultTestData();
                Log.Information("The Database has been reset");
                return Ok("Db has been reset");
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(500, $"Internal Server Error, contact your administrator if continues...");
            }
        }

        private async Task AddDefaultTestData()
        {
            // Create user instance
            var user = new UserDbModel
            {
                Firstname = "Luke",
                Lastname = "Skywalker",
                Email = "luke@skywalker.com",
                Password = "Kode1234!"
            };

            // Create device instances
            var carCharger = new DeviceDbModel
            {
                DeviceName = "Lader til Elbil",
                PowerConsumptionPerHour = 2.5,
                IsOn = true,
                SerialNumber = "Clever1234"
            };
            var heatPump = new DeviceDbModel
            {
                DeviceName = "Varme pumpe",
                PowerConsumptionPerHour = 3.0,
                IsOn = true,
                SerialNumber = "LG1234"
            };
            _dbContext.Devices.AddRange(new[] { carCharger, heatPump });

            // Associate devices with the user using the join table
            var userCarCharger = new UserDeviceDbModel
            {
                Device = carCharger,
                User = user
            };
            var userHeatPump = new UserDeviceDbModel
            {
                Device = heatPump,
                User = user
            };
            _dbContext.UserDevices.AddRange(new[] { userCarCharger, userHeatPump });

            await _dbContext.SaveChangesAsync();
        }
    }
}
