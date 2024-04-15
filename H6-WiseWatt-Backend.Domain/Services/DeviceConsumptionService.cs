using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    public class DeviceConsumptionService : IDeviceConsumptionService
    {
        public ConsumptionStatisticsEntity CalculateStatistics(List<IoTDeviceBaseEntity> devices)
        {
            var statistics = new ConsumptionStatisticsEntity();
            double totalConsumption = 0;

            foreach (var device in devices)
            {
                var dailyUsage = CalculateDailyUsage(device);
                statistics.DailyConsumptionByDevice.Add(device.DeviceName, dailyUsage);
                totalConsumption += dailyUsage;

                var hourlyUsage = CalculateHourlyUsage(device);
                statistics.HourlyConsumptionByDevice.Add(device.DeviceName, hourlyUsage);
            }

            statistics.TotalDailyConsumption = totalConsumption;
            foreach (var device in statistics.DailyConsumptionByDevice)
            {
                double percentage = (device.Value / totalConsumption) * 100;
                statistics.DailyPercentageByDevice.Add(device.Key, percentage);
            }

            return statistics;
        }

        private double CalculateDailyUsage(IoTDeviceBaseEntity device)
        {
            TimeSpan duration = device.OffTime - device.OnTime;
            if (device.OffTime < device.OnTime) // Over midnight case
            {
                duration += TimeSpan.FromDays(1);
            }
            return duration.TotalHours * device.EnergyConsumption;
        }

        private List<double> CalculateHourlyUsage(IoTDeviceBaseEntity device)
        {
            var hourlyConsumption = new List<double>(new double[24]);
            int startHour = device.OnTime.Hours;
            int endHour = device.OffTime.Hours;

            if (endHour < startHour) // Over midnight case
            {
                endHour += 24;
            }

            for (int i = startHour; i <= endHour; i++)
            {
                int hourIndex = i % 24;
                hourlyConsumption[hourIndex] = device.EnergyConsumption;
            }

            return hourlyConsumption;
        }
    }
}
