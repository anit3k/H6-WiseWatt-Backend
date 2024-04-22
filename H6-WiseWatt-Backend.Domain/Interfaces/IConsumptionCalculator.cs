using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    /// <summary>
    /// Analyze electricity consumption for IoT devices. It integrates device management to get users devices
    /// and electricity pricing, providing a suite of methods to compute and retrieve consumption data.
    /// </summary>
    public interface IConsumptionCalculator
    {
        /// <summary>
        /// Returns a list of daily consumption summaries for a specific user. 
        /// The summary includes the device name, daily consumption in kWh, and the associated cost. 
        /// It also calculates a total consumption and cost.
        /// </summary>
        /// <param name="userGuid">Current users unique Id</param>
        /// <returns>list of daily consumption summaries for a specific user</returns>
        Task<Dictionary<string, double>> GetDailyPercentageByDevice(string userGuid);

        /// <summary>
        /// Returns a dictionary mapping each device's name to its percentage share of total daily consumption for a given user.
        /// </summary>
        /// <param name="userGuid">Current users unique Id</param>
        /// <returns>Dictionary mapping each device's name to its percentage share of total daily consumption</returns>
        Task<Dictionary<string, List<double>>> GetHourlyConsumptionByDevice(string userGuid);

        /// <summary>
        /// Returns a dictionary mapping each device's name to its hourly consumption over a 24-hour period.
        /// </summary>
        /// <param name="userGuid">Current users unique Id</param>
        /// <returns>Dictionary mapping each device's name to its hourly consumption over a 24-hour</returns>
        Task<List<Tuple<string, double, double>>> GetSummaryOfDailyConsumption(string userGuid);
    }
}
