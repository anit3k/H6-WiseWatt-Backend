using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Api.Utils;
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
        private readonly IElectricPriceService _electricPriceService;
        private readonly AuthenticationUtility _utility;

        public DashboardController(IDeviceService deviceService, IDeviceConsumptionService deviceConsumptionService, IElectricPriceService electricPriceService, AuthenticationUtility utility)
        {
            _deviceService = deviceService;
            _deviceConsumptionService = deviceConsumptionService;
            _electricPriceService = electricPriceService;
            _utility = utility;
        }

        

        #region Get Daily Percentage
        [HttpGet]
        [Route("api/dashboard/daily-percentage")]
        public async Task<IActionResult> GetPercentage()
        {
            try
            {
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
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
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var deviceEntities = await GetCurrentUserDevices(userGuid);
                var deviceData = _deviceConsumptionService.GetHourlyConsumptionByDevice(deviceEntities);

                var consumptionDtos = deviceData.Select(d => new HourlyConsumptionDTO
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
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
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

        #region Get Electricity Prices
        [HttpGet]
        [Route("api/dashboard/prices")]
        public async Task<IActionResult> GetElectrivPrices()
        {
            try
            {
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var deviceEntities = await GetCurrentUserDevices(userGuid);
                var prices = await _electricPriceService.GetElectricityPricesAsync();
                var today = DateTime.Today;
                var filteredPrices = prices.Where(p => p.TimeStamp.Date >= today)
                                   .OrderBy(p => p.TimeStamp)
                                   .Select(p => new ElectricityPriceDTO
                                   {
                                       TimeStamp = p.TimeStamp,
                                       PricePerKwh = p.PricePerKwh,
                                       TransportAndDuties = p.TransportAndDuties,
                                       TotalPrice = p.TotalPrice
                                   }).ToList();

                return Ok(filteredPrices);
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
            var result = await _deviceService.GetDevices(userId);
            return result;
        }       
    }
}
