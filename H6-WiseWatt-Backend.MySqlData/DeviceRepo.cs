using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Models;
using H6_WiseWatt_Backend.MySqlData.Utils;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class DeviceRepo : IDeviceRepo
    {
        #region fields
        private readonly MySqlDbContext _dbContext;
        private readonly DeviceDbMapper _deviceDbMapper;
        #endregion

        #region Constructor
        public DeviceRepo(MySqlDbContext dbContext, DeviceDbMapper deviceDbMapper)
        {
            _dbContext = dbContext;
            _deviceDbMapper = deviceDbMapper;
        }
        #endregion

        #region Public Methods
        public async Task CreateDevice(IoTDeviceBaseEntity device)
        {
            var deviceModel = _deviceDbMapper.MapToDeviceDbModel(device);
            _dbContext.Add(deviceModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IoTDeviceBaseEntity> GetDevice(string serialNo)
        {
            var dbDevice = await _dbContext.Devices.Where(s => s.Serial == serialNo).FirstOrDefaultAsync();
            if (dbDevice == null) { return null; }
            return _deviceDbMapper.MapToDeviceEntity(dbDevice);
        }
        public async Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid)
        {
            var deviceModels = await _dbContext.Set<DeviceDbModel>()
                                         .Where(d => d.UserGuid == userGuid)
                                         .ToListAsync();

            var devices = deviceModels.Select(dm => _deviceDbMapper.MapToDeviceEntity(dm)).ToList();
            return devices;
        }


        public async Task UpdateDevice(IoTDeviceBaseEntity device)
        {
            var result = _deviceDbMapper.MapToDeviceDbModel(device);
            var deviceModel = await _dbContext.Devices.Where(d => d.Serial == device.Serial.ToString() && d.UserGuid == device.UserGuid.ToString()).FirstOrDefaultAsync();

            if (deviceModel != null)
            {
                deviceModel.UserGuid = result.UserGuid;
                deviceModel.DeviceType = result.DeviceType;
                deviceModel.DeviceName = result.DeviceName;
                deviceModel.Serial = result.Serial;
                deviceModel.IsManuallyOperated = result.IsManuallyOperated;
                deviceModel.IsOn = result.IsOn;
                deviceModel.EnergyConsumption = result.EnergyConsumption;
                deviceModel.OnTime = result.OnTime;
                deviceModel.OffTime = result.OffTime;
                deviceModel.Degree = result.Degree;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteDevice(IoTDeviceBaseEntity device)
        {
            var deviceModel = await _dbContext.Set<DeviceDbModel>()
                                        .FirstOrDefaultAsync(d => d.Serial == device.Serial);
            if (deviceModel != null)
            {
                _dbContext.Remove(deviceModel);
                await _dbContext.SaveChangesAsync();
            }
        }
        #endregion
    }
}
