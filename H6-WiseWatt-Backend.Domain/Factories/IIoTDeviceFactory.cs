using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Factories
{
    public interface IIoTDeviceFactory
    {
        IoTDeviceBaseEntity CreateDevice(string type, string name = null);
    }
}
