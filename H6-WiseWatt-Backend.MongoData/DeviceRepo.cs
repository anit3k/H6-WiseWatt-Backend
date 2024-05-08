using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MongoData.Models;
using H6_WiseWatt_Backend.MongoData.Utils;
using MongoDB.Driver;

namespace H6_WiseWatt_Backend.MongoData
{
    public class DeviceRepo : IDeviceRepo
    {
        private readonly MongoDbContext _dbContext;
        private readonly DeviceDbMapper _deviceDbMapper;

        public DeviceRepo(MongoDbContext dbContext, DeviceDbMapper deviceDbMapper)
        {
            _dbContext = dbContext;
            _deviceDbMapper = deviceDbMapper;
        }

        public async Task CreateDevice(IoTDeviceBaseEntity device)
        {
            var deviceModel = _deviceDbMapper.MapToDeviceDbModel(device);
            await _dbContext.Devices.InsertOneAsync(deviceModel);
        }

        public async Task<IoTDeviceBaseEntity> GetDevice(string serialNo)
        {
            var filter = Builders<DeviceDbModel>.Filter.Eq("Serial", serialNo);
            var dbDevice = await _dbContext.Devices.Find(filter).FirstOrDefaultAsync();
            return dbDevice != null ? _deviceDbMapper.MapToDeviceEntity(dbDevice) : null;
        }

        public async Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid)
        {
            var filter = Builders<DeviceDbModel>.Filter.Eq("UserGuid", userGuid);
            var deviceModels = await _dbContext.Devices.Find(filter).ToListAsync();
            return deviceModels.Select(dm => _deviceDbMapper.MapToDeviceEntity(dm)).ToList();
        }

        public async Task UpdateDevice(IoTDeviceBaseEntity device)
        {
            var filter = Builders<DeviceDbModel>.Filter.Eq("Serial", device.Serial) & Builders<DeviceDbModel>.Filter.Eq("UserGuid", device.UserGuid);
            var deviceModel = await _dbContext.Devices.Find(filter).FirstOrDefaultAsync();

            if (deviceModel != null)
            {
                var update = _deviceDbMapper.MapToDeviceDbModel(device);
                update.Id = deviceModel.Id;
                await _dbContext.Devices.ReplaceOneAsync(filter, update);
            }
        }

        public async Task DeleteDevice(IoTDeviceBaseEntity device)
        {
            var filter = Builders<DeviceDbModel>.Filter.Eq("Serial", device.Serial);
            await _dbContext.Devices.DeleteOneAsync(filter);
        }
    }
}
