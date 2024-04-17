using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IDeviceFactory
    {
        IoTDeviceBaseEntity CreateDevice(string type, string name = null);
        List<IoTDeviceBaseEntity> CreateDefaultDevices();
    }
}
