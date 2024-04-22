using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    /// <summary>
    /// responsible for managing IoT devices in the domain layer. It implements the IDeviceManager interface, 
    /// providing methods to interact with IoT devices, such as retrieving, updating, and managing device status. 
    /// This class acts as a bridge between the device repository and the application logic, handling business rules and additional processing.
    /// </summary>
    public class DeviceManager : IDeviceManager
    {
        #region fields
        private readonly IDeviceRepo _deviceRepo;
        #endregion

        #region Constructor
        public DeviceManager(IDeviceRepo deviceRepo)
        {
            _deviceRepo = deviceRepo;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Asynchronously retrieves a specific IoT device by its serial number. 
        /// If the device is found, it updates its "on" status based on the current time and returns the device; otherwise, it returns null.
        /// </summary>
        /// <param name="serialNo">Device serial number</param>
        /// <returns>IoTDeviceBaseEntity</returns>
        public async Task<IoTDeviceBaseEntity> GetDevice(string serialNo)
        {
            var device = await _deviceRepo.GetDevice(serialNo);
            if (device == null) { return null; }
            UpdateOnStatus(device, DateTime.Now.TimeOfDay);
            return device;
        }

        /// <summary>
        /// Asynchronously retrieves a list of IoT devices associated with a specific users unique identifier. 
        /// It iterates through the devices, updating their "on" status based on the current time.
        /// </summary>
        /// <param name="userGuid">A string representation of the users unique identifier</param>
        /// <returns>A list of IoTDeviceBaseEntity</returns>
        public async Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid)
        {
            var devices = await _deviceRepo.GetDevices(userGuid);
            foreach (var device in devices)
            {
                UpdateOnStatus(device, DateTime.Now.TimeOfDay);
            }
            return devices;
        }

        /// <summary>
        /// Asynchronously updates a specific IoT device. 
        /// Before updating, it refreshes the device's "on" status based on the current time.
        /// </summary>
        /// <param name="device">Specific device</param>
        public async Task UpdateDevice(IoTDeviceBaseEntity device)
        {
            UpdateOnStatus(device, DateTime.Now.TimeOfDay);
            await _deviceRepo.UpdateDevice(device);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Adjusts the "on" status of a device based on whether it is manually operated and its scheduled times (OnTime and OffTime). 
        /// This method handles different scenarios, including devices that operate over midnight.
        /// </summary>
        /// <param name="device">Current device</param>
        /// <param name="currentTime">Current time</param>
        private void UpdateOnStatus(IoTDeviceBaseEntity device, TimeSpan currentTime)
        {
            if (device.IsManuallyOperated)
            {
                device.IsOn = true;
            }
            else
            {
                if (device.OnTime <= device.OffTime)
                {
                    device.IsOn = (currentTime >= device.OnTime && currentTime <= device.OffTime);
                }
                else
                {
                    // Over midnight case
                    device.IsOn = (currentTime >= device.OnTime || currentTime <= device.OffTime); 
                }
            }
        }
        #endregion
    }
}
