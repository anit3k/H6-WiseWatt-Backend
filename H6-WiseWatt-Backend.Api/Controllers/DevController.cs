using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData;
using H6_WiseWatt_Backend.MySqlData.Models;
using H6_WiseWatt_Backend.Security.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly MySqlDbContext _dbContext;
        private readonly IPasswordHasher _passwordService;
        private readonly IDeviceFactory _deviceFactory;
        private readonly IDeviceRepo _deviceRepo;
        private readonly IElectricPriceService _priceService;

        public DevController(MySqlDbContext dbContext,IPasswordHasher passwordService, IDeviceFactory deviceFactory, IDeviceRepo deviceStorageRepo, IElectricPriceService priceService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _deviceFactory = deviceFactory;
            _deviceRepo = deviceStorageRepo;
            _priceService = priceService;
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
                 var temp = await _priceService.GetElectricityPricesAsync();

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
            var salt = _passwordService.GenerateSalt();
            var passwordHash = _passwordService.HashPasswordWithSalt("Kode1234!", salt);
            var user = new UserDbModel
            {
                Firstname = "John",
                Lastname = "Doe",
                Email = "john@test.io",
                PasswordHash = passwordHash,
                Salt = salt,
                UserGuid = "f10774ec-bc8b-40a6-9049-32634363e298"
            };
            _dbContext.Users.Add(user); 
            await _dbContext.SaveChangesAsync();

            var devices = _deviceFactory.CreateDefaultDevices();

            foreach (var device in devices)
            {
                device.UserGuid = user.UserGuid;
                device.Serial = GetStaticSerialForTestUser(device.DeviceType);
                await _deviceRepo.CreateDevice(device);
            }
        }

        private string GetStaticSerialForTestUser(IoTUnit deviceType)
        {
            switch (deviceType)
            {
                case IoTUnit.Dishwasher:
                    return "Bosch-21bc9772";
                    break;
                case IoTUnit.Dryer:
                    return "Mille-1d726d91";
                    break;
                case IoTUnit.CarCharger:
                    return "Clever-ca620c26";
                    break;
                case IoTUnit.HeatPump:
                    return "LG-5ebc34fe";
                    break;
                case IoTUnit.WashingMachine:
                    return "Blomberg-dd273f05";
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
