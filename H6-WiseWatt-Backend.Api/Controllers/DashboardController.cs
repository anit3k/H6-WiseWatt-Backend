using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Api.Utils;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    /// <summary>
    /// Responsible for providing various energy consumption and electricity pricing data endpoints for authenticated users. 
    /// It interacts with services that calculate device consumption and retrieve electricity prices to deliver detailed information to clients.
    /// </summary>
    [Authorize]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IConsumptionCalculator _consumptionCalculator;
        private readonly IElectricPriceService _electricPriceService;
        private readonly AuthenticationUtility _utility;

        public DashboardController(IConsumptionCalculator deviceConsumptionService, IElectricPriceService electricPriceService, AuthenticationUtility utility)
        {
            _consumptionCalculator = deviceConsumptionService;
            _electricPriceService = electricPriceService;
            _utility = utility;
        }

        #region Get Daily Percentage
        /// <summary>
        /// A GET endpoint that returns the daily percentage of energy consumption for each device. It extracts the user GUID from the token and validates it. 
        /// If the user is invalid, it returns a 400 Bad Request; otherwise, it fetches the data and returns the formatted result.
        /// </summary>
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

                var result = await _consumptionCalculator.GetDailyPercentageByDevice(userGuid);
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
        /// <summary>
        /// A GET endpoint that returns the hourly consumption data for each device. It validates the user, then retrieves and formats the data.
        /// </summary>
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

                var deviceData = await _consumptionCalculator.GetHourlyConsumptionByDevice(userGuid);

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
        /// <summary>
        ///  A GET endpoint that provides a summary of daily consumption, including the total consumption and cost for all devices. 
        ///  It validates the user and retrieves and formats the summary data.
        /// </summary>
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

                var data = await _consumptionCalculator.GetSummaryOfDailyConsumption(userGuid);

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
        /// <summary>
        /// A GET endpoint that fetches and returns electricity prices, filtering by today's date and ordering by timestamp. 
        /// It validates the user, retrieves, and formats the electricity prices into a list.
        /// </summary>
        /// <returns></returns>
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
    }
}
