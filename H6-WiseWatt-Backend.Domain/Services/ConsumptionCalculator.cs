﻿using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    public class ConsumptionCalculator : IConsumptionCalculator
    {
        private readonly IDeviceManager _deviceManager;
        private readonly IElectricPriceService _electricPriceService;

        public ConsumptionCalculator(IDeviceManager deviceManager, IElectricPriceService electricPriceService)
        {
            _deviceManager = deviceManager;
            _electricPriceService = electricPriceService;
        }

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

        private async Task<List<IoTDeviceBaseEntity>> GetUserDevices(string userGuid)
        {
            return await _deviceManager.GetDevices(userGuid);
        }

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

        private double CalculateDailyUsage(IoTDeviceBaseEntity device)
        {
            if (device.IsManuallyOperated)
            {
                return 24 * device.EnergyConsumption;  // Device runs for 24 hours if manually operated.
            }
            else
            {
                if (device.OnTime == device.OffTime)
                {
                    return 0; // If OnTime and OffTime are the same, no energy is consumed.
                }

                TimeSpan duration = device.OffTime - device.OnTime;
                if (device.OffTime < device.OnTime) // Over midnight case
                {
                    duration += TimeSpan.FromDays(1);
                }
                return duration.TotalHours * device.EnergyConsumption;
            }
        }


        private List<double> CalculateHourlyUsage(IoTDeviceBaseEntity device)
        {
            var hourlyConsumption = new List<double>(new double[24]);
            if (device.OnTime == device.OffTime && !device.IsManuallyOperated)
            {
                return hourlyConsumption; // No consumption if times are the same and not manually operated.
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

            int startHour = device.OnTime.Hours;
            int endHour = device.OffTime.Hours;
            if (endHour < startHour) // Over midnight case
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
    }
}