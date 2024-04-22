using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for creating IoT devices within the domain layer. 
    /// It specifies the methods that a class must implement to provide device creation functionality. 
    /// Interfaces like this allow for abstraction and decoupling, enabling flexible implementation without being tied to a specific class.
    /// </summary>
    public interface IDeviceFactory
    {
        /// <summary>
        /// Creates an instance of an IoT device based on the specified type and optional name. 
        /// It uses a switch-case structure to handle different device types, 
        /// such as dishwashers, dryers, car chargers, heat pumps, and washing machines. 
        /// If an unknown type is provided, it throws an ArgumentException.
        /// </summary>
        /// <param name="type">string representation of the device type</param>
        /// <param name="name">name of the device</param>
        /// <returns>A specific child of IoTDeviceBaseEntity</returns>
        IoTDeviceBaseEntity CreateDevice(string type, string name = null);
        /// <summary>
        /// Creates a list of default IoT devices, providing a set of commonly used devices with predefined characteristics. 
        /// This method is used for initializing test data or setting up a simulated environment.
        /// </summary>
        /// <returns>The default list of IoTDeviceBaseEntity</returns>
        List<IoTDeviceBaseEntity> CreateDefaultDevices();
    }
}
