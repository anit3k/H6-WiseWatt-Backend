using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IDeviceService
    {
        Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid);
        Task UpdateDevice(IoTDeviceBaseEntity device);
        Task<IoTDeviceBaseEntity> GetDevice(string serialNo);

    }
}
