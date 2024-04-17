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
        private readonly IDeviceService _deviceService;
        private readonly IDeviceConsumptionService _deviceConsumptionService;

        public DashboardController(IDeviceService deviceService, IDeviceConsumptionService deviceConsumptionService)
        {
            _deviceService = deviceService;
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
                return Ok(deviceEntities.Select(dm => MapToDeviceDto(dm)).ToList());
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
                var userGuid = GetUserGuidFromToken();
                if (ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }
                var deviceEntity = MapToDeviceEntity(device);

                await _deviceService.UpdateDevice(deviceEntity);
                return Ok("Device Updated");
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }
        #endregion

        #region Get Daily Percentage
        [HttpGet]
        [Route("api/dashboard/daily-percentage")]
        public async Task<IActionResult> GetPercentage()
        {
            try
            {
                var userGuid = GetUserGuidFromToken();
                if (ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var deviceEntities = await GetCurrentUserDevices(userGuid);

                var result = _deviceConsumptionService.GetDailyPercentageByDevice(deviceEntities);
                var formattedResponse = result.Select(kvp => new PercentageDTO
                {
                    Value = Math.Round(kvp.Value, 2),
                    Name = kvp.Key
                }).ToList();

                return Ok(formattedResponse);
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }
        #endregion

        #region Get Hourly Consumption
        [HttpGet]
        [Route("api/dashboard/hourly-consumption")]
        public async Task<IActionResult> GetHourlyConsumption()
        {
            try
            {
                var userGuid = GetUserGuidFromToken();
                if (ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var deviceEntities = await GetCurrentUserDevices(userGuid);
                var deviceData = _deviceConsumptionService.GetHourlyConsumptionByDevice(deviceEntities);

                var consumptionDtos = deviceData.Select(d => new HourlyConsumptionDto
                {
                    Name = d.Key,
                    Data = d.Value.Select(x => Math.Round(x, 2)).ToList()
                }).ToList();


                return Ok(consumptionDtos);
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }
        #endregion

        #region Get Summary Daily Consumption
        [HttpGet]
        [Route("api/dashboard/daily-summary")]
        public async Task<IActionResult> GetSummaryDailyConsumption()
        {
            try
            {
                var userGuid = GetUserGuidFromToken();
                if (ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var deviceEntities = await GetCurrentUserDevices(userGuid);
                var data = await _deviceConsumptionService.GetSummaryOfDailyConsumption(deviceEntities);

                var formattedItems = data.Select(i => new
                {
                    Name = i.Item1, 
                    CurrentConsumption = Math.Round(i.Item2, 2),
                    Cost = Math.Round(i.Item3, 2)
                }).ToList();

                return Ok(formattedItems);
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
            var result = await _deviceService.GetDevices(userId);
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
                IsManuallyOperated = entity.IsManuallyOperated,
                EnergyConsumption = entity.EnergyConsumption,
                OnTime = entity.OnTime,
                OffTime = entity.OffTime,
                IsOn = entity.IsOn,
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
            IoTDeviceBaseEntity entity = model.DeviceType switch
            {
                "Dishwasher" => new DishwasherEntity(),
                "Dryer" => new DryerEntity(),
                "CarCharger" => new ElectricCarChargerEntity(),
                "HeatPump" => new HeatPumpEntity(),
                "WashingMachine" => new WashingMachineEntity(),
                _ => throw new ArgumentException("Unknown device type", nameof(model.DeviceType))
            };

            entity.UserGuid = model.UserGuid;
            entity.DeviceName = model.DeviceName;
            entity.Serial = model.Serial;
            entity.IsManuallyOperated = model.IsManuallyOperated;
            entity.EnergyConsumption = model.EnergyConsumption;
            entity.OnTime = model.OnTime;
            entity.OffTime = model.OffTime;
            entity.IsOn = model.IsOn;


            if (entity is HeatPumpEntity heatPump && model.Degree.HasValue)
            {
                heatPump.Degree = model.Degree.Value;
            }

            return entity;
        }
    }
}
