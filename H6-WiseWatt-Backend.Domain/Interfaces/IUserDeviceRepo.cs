using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IUserDeviceRepo
    {
        Task<List<DeviceEntity>> GetDevices(string email);
        Task<bool> SetDeviceState(DeviceEntity device);
    }
}
