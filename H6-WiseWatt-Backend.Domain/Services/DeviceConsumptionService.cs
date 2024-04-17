using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    public class DeviceConsumptionService : IDeviceConsumptionService
    {
        public List<Tuple<string, double, double>> GetSummaryOfDailyConsumption(List<IoTDeviceBaseEntity> devices)
        {
            var result = new List<Tuple<string, double, double>>();
            double totalConsumption = 0;
            foreach (var device in devices)
            {
                var dailyUsage = CalculateDailyUsage(device);
                result.Add(new Tuple<string, double, double>(device.DeviceName, dailyUsage, dailyUsage * 2));
                totalConsumption += dailyUsage;
            }
            result.Add(new Tuple<string, double, double>("Total", totalConsumption, totalConsumption * 2));
            return result;
        }

        public Dictionary<string, double> GetDailyPercentageByDevice(List<IoTDeviceBaseEntity> devices)
        {
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

        public Dictionary<string, List<double>> GetHourlyConsumptionByDevice(List<IoTDeviceBaseEntity> devices)
        {
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
