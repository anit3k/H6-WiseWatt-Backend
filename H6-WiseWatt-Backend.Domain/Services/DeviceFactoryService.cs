﻿using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    public class DeviceFactoryService : IDeviceFactory
    {
        private readonly Random _random = new Random();

        public IoTDeviceBaseEntity CreateDevice(string type, string name = null)
        {
            switch (type)
            {
                case "Dishwasher":
                    return new DishwasherEntity
                    {
                        DeviceName = name ?? "Opvaskemaskine",
                        IsManuallyOperated = false,
                        IsOn = false,
                        EnergyConsumption = GetRandomConsumption(1.5, 0.8),
                        Serial = "Bosch-" + GetRandomString(),
                        OnTime = new TimeSpan(21, 0, 0),
                        OffTime = new TimeSpan(23, 30, 0),
                    };
                case "Dryer":
                    return new DryerEntity
                    {
                        DeviceName = name ?? "Tørretumbler",
                        IsManuallyOperated = false,
                        IsOn = false,
                        EnergyConsumption = GetRandomConsumption(4.5, 2.5),
                        Serial = "Mille-" + GetRandomString(),
                        OnTime = new TimeSpan(9, 0, 0),
                        OffTime = new TimeSpan(12, 0, 0),
                    };
                case "CarCharger":
                    return new ElectricCarChargerEntity
                    {
                        DeviceName = name ?? "Ladestander",
                        IsManuallyOperated = false,
                        IsOn = false,
                        EnergyConsumption = GetRandomConsumption(11, 7),
                        Serial = "Clever-" + GetRandomString(),
                        OnTime = new TimeSpan(22, 0, 0),
                        OffTime = new TimeSpan(7, 0, 0),
                    };
                case "HeatPump":
                    return new HeatPumpEntity
                    {
                        DeviceName = name ?? "Varmepumpe",
                        IsManuallyOperated = false,
                        IsOn = false,
                        EnergyConsumption = GetRandomConsumption(5, 2),
                        Serial = "LG-" + GetRandomString(),
                        OnTime = new TimeSpan(16, 0, 0),
                        OffTime = new TimeSpan(23, 0, 0),
                        Degree = 20,
                    };
                case "WashingMachine":
                    return new WashingMachineEntity
                    {
                        DeviceName = name ?? "Vaskemaskine",
                        IsManuallyOperated = false,
                        IsOn = false,
                        EnergyConsumption = GetRandomConsumption(1.5, 0.5),
                        Serial = "Blomberg-" + GetRandomString(),
                        OnTime = new TimeSpan(02, 0, 0),
                        OffTime = new TimeSpan(05, 0, 0),
                    };
                default:
                    throw new ArgumentException("Unknown device type", nameof(type));
            }
        }

        public List<IoTDeviceBaseEntity> CreateDefaultDevices()
        {
            var devices = new List<IoTDeviceBaseEntity>
            {
                CreateDevice("Dishwasher"),
                CreateDevice("Dryer"),
                CreateDevice("CarCharger"),
                CreateDevice("HeatPump"),
                CreateDevice("WashingMachine")
            };
            return devices;
        }

        private double GetRandomConsumption(double max, double min)
        {
            return _random.NextDouble() * (max - min) + min;
        }

        private string GetRandomString()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}