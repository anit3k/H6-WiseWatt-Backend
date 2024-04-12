using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class UserDeviceRepo : IUserDeviceRepo
    {
        private readonly MySqlDbContext _dbContext;
        public UserDeviceRepo(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }        

        public async Task<List<DeviceEntity>> GetDevices(string email)
        {
            var result = await _dbContext.UserDevices
                .Where( u => u.User.Email == email)
                .Select( d => MapDeviceFromDbToEntity(d.Device))
                .ToListAsync();

            return result;
        }

        public async Task<bool> SetDeviceState(DeviceEntity device)
        {
            var result = await _dbContext.Devices
                .Where( d => d.SerialNumber == device.SerialNumber)
                .FirstOrDefaultAsync();

            return result.IsOn;
        }

        private static DeviceEntity MapDeviceFromDbToEntity(DeviceDbModel dbModel)
        {
            return new DeviceEntity
            {
                
                DeviceName= dbModel.DeviceName,
                PowerConsumptionPerHour = dbModel.PowerConsumptionPerHour,
                IsOn = dbModel.IsOn,
                SerialNumber = dbModel.SerialNumber
            };
        }
    }
}
