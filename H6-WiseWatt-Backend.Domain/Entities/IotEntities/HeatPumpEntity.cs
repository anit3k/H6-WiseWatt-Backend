namespace H6_WiseWatt_Backend.Domain.Entities.IotEntities
{
    public class HeatPumpEntity : IoTDeviceBaseEntity
    {
        public override IoTUnit DeviceType => IoTUnit.HeatPump;
        public int Degree { get; set; }
    }
}
