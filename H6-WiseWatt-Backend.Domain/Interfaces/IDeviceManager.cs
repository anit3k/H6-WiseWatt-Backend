using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for managing IoT devices within the domain layer. 
    /// It specifies a set of methods for retrieving, updating, and interacting with device-related data. 
    /// By using an interface, the specific implementation of these operations can be decoupled from other components, allowing for flexibility and easier testing.
    /// </summary>
    public interface IDeviceManager
    {
        /// <summary>
        /// Asynchronously retrieves a list of IoTDeviceBaseEntity objects associated with a specific user GUID. This method is used to fetch all devices for a given user.
        /// </summary>
        /// <param name="userGuid">Users unique identifier</param>
        /// <returns>A list of IoT devices for the current user</returns>
        Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid);

        /// <summary>
        /// Asynchronously updates an IoT device. 
        /// This method takes an instance of IoTDeviceBaseEntity, allowing modifications to be saved to the underlying data store.
        /// </summary>
        /// <param name="device">Specific device</param>
        Task UpdateDevice(IoTDeviceBaseEntity device);

        /// <summary>
        /// Asynchronously retrieves a specific IoT device by its serial number. This method is used to fetch a single device based on its unique identifier.
        /// </summary>
        /// <param name="serialNo">Unique serial from specific device</param>
        /// <returns>The specific deivce</returns>
        Task<IoTDeviceBaseEntity> GetDevice(string serialNo);
    }
}
