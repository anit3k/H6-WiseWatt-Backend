using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    /// <summary>
    /// Responsible for creating instances of various IoT devices. 
    /// It implements the IDeviceFactory interface and provides methods to create individual IoT devices or a list of default devices, 
    /// with random characteristics to simulate variability in a real-world scenario.
    /// </summary>
    public class DeviceFactoryService : IDeviceFactory
    {
        #region fields
        private readonly Random _random = new Random();
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates an instance of an IoT device based on the specified type and optional name. 
        /// It uses a switch-case structure to handle different device types, 
        /// such as dishwashers, dryers, car chargers, heat pumps, and washing machines. 
        /// If an unknown type is provided, it throws an ArgumentException.
        /// </summary>
        /// <param name="type">string representation of the device type</param>
        /// <param name="name">name of the device</param>
        /// <returns>A specific child of IoTDeviceBaseEntity</returns>
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

        /// <summary>
        /// Creates a list of default IoT devices, providing a set of commonly used devices with predefined characteristics. 
        /// This method is used for initializing test data or setting up a simulated environment.
        /// </summary>
        /// <returns>The default list of IoTDeviceBaseEntity</returns>
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
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns a random energy consumption value between the specified minimum and maximum limits. 
        /// This is used to simulate variability in device energy consumption.
        /// </summary>
        /// <param name="max">Minimum consumption value of the specific device</param>
        /// <param name="min">maximum consumption value of the specific device</param>
        /// <returns>The consumption value</returns>
        private double GetRandomConsumption(double max, double min)
        {
            return _random.NextDouble() * (max - min) + min;
        }
        /// <summary>
        /// Returns a random string derived from a GUID, providing unique identifiers for device serial numbers.
        /// </summary>
        /// <returns></returns>
        private string GetRandomString()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }
        #endregion
    }
}
