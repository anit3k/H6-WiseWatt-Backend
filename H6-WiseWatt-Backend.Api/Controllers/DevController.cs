﻿using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Factories;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly MySqlDbContext _dbContext;
        private readonly IIoTDeviceFactory _deviceFactory;
        private readonly IDeviceRepo _deviceStorageRepo;

        public DevController(MySqlDbContext dbContext, IIoTDeviceFactory deviceFactory, IDeviceRepo deviceStorageRepo)
        {
            _dbContext = dbContext;
            _deviceFactory = deviceFactory;
            _deviceStorageRepo = deviceStorageRepo;
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
                await AddDefaultTestData2();
                Log.Information("The Database has been reset");
                return Ok("Db has been reset");
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(500, $"Internal Server Error, contact your administrator if continues.../n" + ex.Message);
            }
        }

        private async Task AddDefaultTestData2()
        {
            var user = new UserDbModel
            {
                Firstname = "Luke",
                Lastname = "Skywalker",
                Email = "luke@skywalker.com",
                Password = "Kode1234!",
                UserGuid = "f10774ec-bc8b-40a6-9049-32634363e298"
            };
            _dbContext.Users.Add(user); 
            await _dbContext.SaveChangesAsync();

            var devices = new List<IoTDeviceBaseEntity>
            {
                _deviceFactory.CreateDevice("Dishwasher"),
                _deviceFactory.CreateDevice("Dryer"),
                _deviceFactory.CreateDevice("CarCharger"),
                _deviceFactory.CreateDevice("HeatPump"),
                _deviceFactory.CreateDevice("WashingMachine")
            };

            foreach (var device in devices)
            {
                device.UserGuid = user.UserGuid;
                await _deviceStorageRepo.CreateDevice(device);
            }
        }        
    }
}
