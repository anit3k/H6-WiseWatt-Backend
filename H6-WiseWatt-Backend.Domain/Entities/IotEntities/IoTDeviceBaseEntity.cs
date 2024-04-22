namespace H6_WiseWatt_Backend.Domain.Entities.IotEntities
{
    /// <summary>
    /// Abstract base class that represents common attributes and behaviors for IoT devices. 
    /// This class serves as the foundation for defining specific types of IoT devices within the domain layer, 
    /// ensuring consistency and a shared structure among all device entities.
    /// </summary>
    public abstract class IoTDeviceBaseEntity
    {
        #region Properties
        public string UserGuid { get; set; }
        public abstract IoTUnit DeviceType { get; }
        public string DeviceName { get; set; }
        public string Serial { get; set; }
        public bool IsManuallyOperated { get; set; }
        public bool IsOn { get; set; }
        public double EnergyConsumption { get; set; }
        public TimeSpan OnTime { get; set; }
        public TimeSpan OffTime { get; set; }
        #endregion
    }
}
