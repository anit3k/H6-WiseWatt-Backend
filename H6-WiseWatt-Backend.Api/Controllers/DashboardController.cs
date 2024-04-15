using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDeviceRepo _userDeviceRepo;
        private readonly IDeviceConsumptionService _deviceConsumptionService;

        public DashboardController(IDeviceRepo userDeviceRepo, IDeviceConsumptionService deviceConsumptionService)
        {
            _userDeviceRepo = userDeviceRepo;
            _deviceConsumptionService = deviceConsumptionService;
        }

        #region GetAllUsersDevices
        [HttpGet]
        [Route("api/dashboard/devices")]
        public async Task<IActionResult> GetDevices()
        {
            try
            {
                var userGuid = GetUserGuidFromToken();
                if (ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var deviceEntities = await GetCurrentUserDevices(userGuid);
                var response = GetResponseDto(deviceEntities);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }

        private DashboardDto GetResponseDto(List<IoTDeviceBaseEntity> deviceEntities)
        {
            var stats = _deviceConsumptionService.CalculateStatistics(deviceEntities);
            var result = new DashboardDto
            {
                Stats = new Statistics
                {
                    DailyPercentageByDevice = stats.DailyPercentageByDevice,
                    DailyConsumptionByDevice = stats.DailyConsumptionByDevice,
                    HourlyConsumptionByDevice = stats.HourlyConsumptionByDevice,
                    TotalDailyConsumption = stats.TotalDailyConsumption
                },
                Devices = deviceEntities.Select(dm => MapToDeviceDto(dm)).ToList()
            };
            return result;
        }

        #endregion

        #region Update Device
        [HttpPost]
        [Route("api/dashboard/updateDevice")]
        public async Task<IActionResult> UpdateDevice(DeviceDto device)
        {
            try
            {
                var userGuid = GetUserGuidFromToken();
                if (ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }
                var deviceEntity = MapToDeviceEntity(device);

                await _userDeviceRepo.UpdateDevice(deviceEntity);
                return Ok("Device Updated");
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }
        #endregion


        private string? GetUserGuidFromToken()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
        }

        private bool ValidateUser(string? userId)
        {
            return userId == null || string.IsNullOrEmpty(userId);
        }
        private async Task<List<IoTDeviceBaseEntity>> GetCurrentUserDevices(string? userId)
        {
            Log.Information($"User {userId} request a list of all devices");
            var result = await _userDeviceRepo.GetDevices(userId);
            return result;
        }

        private DeviceDto MapToDeviceDto(IoTDeviceBaseEntity entity)
        {
            var model = new DeviceDto
            {
                UserGuid = entity.UserGuid,
                DeviceType = entity.DeviceType.ToString(),
                DeviceName = entity.DeviceName,
                Serial = entity.Serial,
                IsOn = entity.IsOn,
                EnergyConsumption = entity.EnergyConsumption,
                OnTime = entity.OnTime,
                OffTime = entity.OffTime,
                Degree = null,
            };

            switch (entity)
            {
                case DishwasherEntity charger:
                    break;
                case DryerEntity dryer:
                    break;
                case ElectricCarChargerEntity charger:
                    break;
                case HeatPumpEntity heatPump:
                    model.Degree = heatPump.Degree;
                    break;
                case WashingMachineEntity charger:
                    break;
            }

            return model;
        }

        private IoTDeviceBaseEntity MapToDeviceEntity(DeviceDto model)
        {
            return model.DeviceType switch
            {
                "Dishwasher" => new DishwasherEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime,
                },
                "Dryer" => new DryerEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime,
                },
                "CarCharger" => new ElectricCarChargerEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime
                },
                "HeatPump" => new HeatPumpEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime,
                    Degree = (int)model.Degree,
                },
                "WashingMachine" => new WashingMachineEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime,
                },
                _ => throw new ArgumentException("Unknown device type", nameof(model.DeviceType))
            };
        }
    }
}
