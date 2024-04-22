namespace H6_WiseWatt_Backend.Domain.Entities.IotEntities
{
    /// <summary>
    /// Specific type of IoT device within the domain layer. 
    /// This class extends the IoTDeviceBaseEntity abstract base class, 
    /// inheriting its common attributes and behaviors while defining the unique aspects of a heat pump IoT device.
    /// </summary>
    public class HeatPumpEntity : IoTDeviceBaseEntity
    {
        public override IoTUnit DeviceType => IoTUnit.HeatPump;
        public int Degree { get; set; }
    }
}
