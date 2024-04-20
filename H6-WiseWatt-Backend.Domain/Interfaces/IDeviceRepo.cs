using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IDeviceRepo
    {
        Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid);
        Task<IoTDeviceBaseEntity> GetDevice(string serialNo);
        Task UpdateDevice(IoTDeviceBaseEntity device);
        Task CreateDevice(IoTDeviceBaseEntity device);
        Task DeleteDevice(IoTDeviceBaseEntity device);
    }
}
