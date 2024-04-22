namespace H6_WiseWatt_Backend.Domain.Entities.IotEntities
{
    /// <summary>
    /// Specific type of IoT device within the domain layer. 
    /// This class extends the IoTDeviceBaseEntity abstract base class, 
    /// inheriting its common attributes and behaviors while defining the unique aspects of a dryer IoT device.
    /// </summary>
    public class DryerEntity : IoTDeviceBaseEntity
    {
        public override IoTUnit DeviceType => IoTUnit.Dryer;
    }
}
