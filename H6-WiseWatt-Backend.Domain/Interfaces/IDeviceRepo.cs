using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for device-related repository operations within the domain layer. 
    /// It specifies the methods for interacting with the data store to retrieve, create, update, and delete IoT device information.
    /// </summary>
    public interface IDeviceRepo
    {
        /// <summary>
        /// Asynchronously retrieves a list of IoT devices associated with a specific user, 
        /// identified by the userGuid. The return type is a list of IoTDeviceBaseEntity.
        /// </summary>
        /// <param name="userGuid">Users unique identifier</param>
        /// <returns>A list IoTDeviceBaseEntity</returns>
        Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid);

        /// <summary>
        /// Asynchronously retrieves a specific IoT device by its serial number. 
        /// If the device is found, it is returned; otherwise, the method returns null.
        /// </summary>
        /// <param name="serialNo">Unique serial number of the IoT device</param>
        /// <returns>IoTDeviceBaseEntity</returns>
        Task<IoTDeviceBaseEntity> GetDevice(string serialNo);

        /// <summary>
        /// Asynchronously updates the given IoT device in the data store. This method saves changes made to a device.
        /// </summary>
        /// <param name="device">Specific device</param>
        Task UpdateDevice(IoTDeviceBaseEntity device);

        /// <summary>
        /// Asynchronously creates a new IoT device in the data store, initializing it with the given IoTDeviceBaseEntity.
        /// </summary>
        /// <param name="device">Specific device</param>
        Task CreateDevice(IoTDeviceBaseEntity device);

        /// <summary>
        /// Asynchronously deletes a given IoT device from the data store.
        /// </summary>
        /// <param name="device">Specific device</param>
        Task DeleteDevice(IoTDeviceBaseEntity device);
    }
}
