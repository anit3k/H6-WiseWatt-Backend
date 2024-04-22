namespace H6_WiseWatt_Backend.Domain.Entities.IotEntities
{
    /// <summary>
    /// Specific type of IoT device within the domain layer. 
    /// This class extends the IoTDeviceBaseEntity abstract base class, 
    /// inheriting its common attributes and behaviors while defining the unique aspects of a washing machine IoT device.
    /// </summary>
    public class WashingMachineEntity : IoTDeviceBaseEntity
    {
        public override IoTUnit DeviceType => IoTUnit.WashingMachine;
    }
}
