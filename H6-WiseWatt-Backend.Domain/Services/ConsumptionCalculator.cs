using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    /// <summary>
    /// Analyze electricity consumption for IoT devices. It integrates device management to get users devices
    /// and electricity pricing, providing a suite of methods to compute and retrieve consumption data.
    /// </summary>
    public class ConsumptionCalculator : IConsumptionCalculator
    {
        private readonly IDeviceManager _deviceManager;
        private readonly IElectricPriceService _electricPriceService;

        public ConsumptionCalculator(IDeviceManager deviceManager, IElectricPriceService electricPriceService)
        {
            _deviceManager = deviceManager;
            _electricPriceService = electricPriceService;
        }

        /// <summary>
        /// Returns a list of daily consumption summaries for a specific user. 
        /// The summary includes the device name, daily consumption in kWh, and the associated cost. 
        /// It also calculates a total consumption and cost.
        /// </summary>
        /// <param name="userGuid">Current users unique Id</param>
        /// <returns>list of daily consumption summaries for a specific user</returns>
        public async Task<List<Tuple<string, double, double>>> GetSummaryOfDailyConsumption(string userGuid)
        {
            var devices = await GetUserDevices(userGuid);
            var prices = await _electricPriceService.GetElectricityPricesAsync(); // Fetch hourly prices.
            var result = new List<Tuple<string, double, double>>();
            double totalConsumption = 0;
            double totalCost = 0;

            foreach (var device in devices)
            {
                var hourlyUsage = CalculateHourlyUsage(device);
                double dailyUsage = 0;
                double dailyCost = 0;

                // Calculate hourly consumption and cost
                for (int hour = 0; hour < 24; hour++)
                {
                    double hourPrice = prices.FirstOrDefault(p => p.TimeStamp.Hour == hour)?.TotalPrice ?? 0;
                    dailyUsage += hourlyUsage[hour];
                    dailyCost += hourlyUsage[hour] * hourPrice;
                }

                result.Add(new Tuple<string, double, double>(device.DeviceName, dailyUsage, dailyCost));
                totalConsumption += dailyUsage;
                totalCost += dailyCost;
            }

            // Add total consumption and cost
            result.Add(new Tuple<string, double, double>("Total", totalConsumption, totalCost));
            return result;
        }

        /// <summary>
        /// Returns a dictionary mapping each device's name to its percentage share of total daily consumption for a given user.
        /// </summary>
        /// <param name="userGuid">Current users unique Id</param>
        /// <returns>Dictionary mapping each device's name to its percentage share of total daily consumption</returns>
        public async Task<Dictionary<string, double>> GetDailyPercentageByDevice(string userGuid)
        {
            var devices = await GetUserDevices(userGuid);
            var result = new Dictionary<string, double>();
            var dailyConsumption = new Dictionary<string, double>();
            double totalConsumption = 0;

            foreach (var device in devices)
            {
                var dailyUsage = CalculateDailyUsage(device);
                dailyConsumption.Add(device.DeviceName, dailyUsage);
                totalConsumption += dailyUsage;
            }

            foreach (var device in dailyConsumption)
            {
                double percentage = (device.Value / totalConsumption) * 100;
                result.Add(device.Key, percentage);
            }

            return result;
        }

        /// <summary>
        /// Returns a dictionary mapping each device's name to its hourly consumption over a 24-hour period.
        /// </summary>
        /// <param name="userGuid">Current users unique Id</param>
        /// <returns>Dictionary mapping each device's name to its hourly consumption over a 24-hour</returns>
        public async Task<Dictionary<string, List<double>>> GetHourlyConsumptionByDevice(string userGuid)
        {
            var devices = await GetUserDevices(userGuid);
            var result = new Dictionary<string, List<double>>();
            foreach (var device in devices)
            {
                var hourlyUsage = CalculateHourlyUsage(device);
                result.Add(device.DeviceName, hourlyUsage);
            }
            return result;
        }

        /// <summary>
        /// A private helper method that fetches the list of devices associated with a given user.
        /// </summary>
        /// <param name="userGuid"Current users unique Id></param>
        /// <returns>List of devices</returns>
        private async Task<List<IoTDeviceBaseEntity>> GetUserDevices(string userGuid)
        {
            return await _deviceManager.GetDevices(userGuid);
        }

        /// <summary>
        /// Computes the daily energy consumption for a given device based on its operation schedule and energy consumption rate.
        /// </summary>
        /// <param name="device">Current IoT Device</param>
        /// <returns>Daily energy consumption</returns>
        private double CalculateDailyUsage(IoTDeviceBaseEntity device)
        {
            // Device runs for 24 hours if manually operated.
            if (device.IsManuallyOperated)
            {
                return 24 * device.EnergyConsumption;  
            }
            else
            {
                // If OnTime and OffTime are the same, no energy is consumed.
                if (device.OnTime == device.OffTime)
                {
                    return 0; 
                }

                TimeSpan duration = device.OffTime - device.OnTime;
                // Over midnight case
                if (device.OffTime < device.OnTime) 
                {
                    duration += TimeSpan.FromDays(1);
                }
                return duration.TotalHours * device.EnergyConsumption;
            }
        }

        /// <summary>
        /// Computes the hourly energy consumption for a given device based on its operation schedule and energy consumption rate.
        /// </summary>
        /// <param name="device">Current device</param>
        /// <returns>Hourly device energy consumption</returns>
        private List<double> CalculateHourlyUsage(IoTDeviceBaseEntity device)
        {
            var hourlyConsumption = new List<double>(new double[24]);
                        
            if (HasNoConsumption(device))
            {
                return hourlyConsumption;
            }

            // Device runs for 24 hours if manually operated.
            if (device.IsManuallyOperated)
            {
                for (var i = 0; i < 24; i++)
                {
                    hourlyConsumption[i] = device.EnergyConsumption;
                }
                return hourlyConsumption;
            }

            // Device runs by timer
            int startHour = device.OnTime.Hours;
            int endHour = device.OffTime.Hours;
            if (IsOverMidnight(startHour, endHour))
            {
                for (int hour = startHour; hour < 24; hour++)
                {
                    hourlyConsumption[hour] = device.EnergyConsumption;
                }
                for (int hour = 0; hour < endHour; hour++)
                {
                    hourlyConsumption[hour] = device.EnergyConsumption;
                }
            }
            else
            {
                for (int hour = startHour; hour < endHour; hour++)
                {
                    hourlyConsumption[hour] = device.EnergyConsumption;
                }
            }

            return hourlyConsumption;
        }

        /// <summary>
        /// No consumption if times are the same and not manually operated.
        /// </summary>
        /// <param name="device">Current Device</param>
        /// <returns>boolean</returns>
        private bool HasNoConsumption(IoTDeviceBaseEntity device)
        {
            return device.OnTime == device.OffTime && !device.IsManuallyOperated;
        }

        /// <summary>
        /// Determines if a device's operating schedule crosses midnight, used to correctly calculate consumption.
        /// </summary>
        /// <param name="startHour"></param>
        /// <param name="endHour"></param>
        /// <returns>true/false</returns>
        private bool IsOverMidnight(int startHour, int endHour)
        {
            return endHour < startHour;
        }
    }
}
