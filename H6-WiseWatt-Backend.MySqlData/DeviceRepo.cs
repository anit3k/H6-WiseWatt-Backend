using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class DeviceRepo : IDeviceRepo
    {
        private readonly MySqlDbContext _dbContext;

        public DeviceRepo(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateDevice(IoTDeviceBaseEntity device)
        {
            var deviceModel = MapToDeviceDbModel(device);
            _dbContext.Add(deviceModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IoTDeviceBaseEntity> GetDevice(string serialNo)
        {
            var dbDevice = await _dbContext.Devices.Where(s => s.Serial == serialNo).FirstOrDefaultAsync();
            return MapToDeviceEntity(dbDevice);
        }
        public async Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid)
        {
            var deviceModels = await _dbContext.Set<DeviceDbModel>()
                                         .Where(d => d.UserGuid == userGuid)
                                         .ToListAsync();

            var devices = deviceModels.Select(dm => MapToDeviceEntity(dm)).ToList();
            return devices;
        }


        public async Task UpdateDevice(IoTDeviceBaseEntity device)
        {
            var result = MapToDeviceDbModel(device);
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

        private DeviceDbModel MapToDeviceDbModel(IoTDeviceBaseEntity entity)
        {
            var model = new DeviceDbModel
            {
                UserGuid = entity.UserGuid,
                DeviceType = entity.DeviceType.ToString(),
                DeviceName = entity.DeviceName,
                Serial = entity.Serial,
                IsManuallyOperated = entity.IsManuallyOperated,                
                IsOn = entity.IsOn,
                EnergyConsumption = entity.EnergyConsumption,
                OnTime = entity.OnTime,
                OffTime = entity.OffTime
            };

            switch (entity)
            {
                case DishwasherEntity charger:
                    break;
                case DryerEntity dryer:
                    break;
                case ElectricCarChargerEntity charger:
                    break;
                case HeatPumpEntity heatPump:
                    model.Degree = heatPump.Degree;
                    break;
                case WashingMachineEntity charger:
                    break;
            }

            return model;
        }

        private IoTDeviceBaseEntity MapToDeviceEntity(DeviceDbModel model)
        {
            IoTDeviceBaseEntity entity = model.DeviceType switch
            {
                "Dishwasher" => new DishwasherEntity(),
                "Dryer" => new DryerEntity(),
                "CarCharger" => new ElectricCarChargerEntity(),
                "HeatPump" => new HeatPumpEntity(),
                "WashingMachine" => new WashingMachineEntity(),
                _ => throw new ArgumentException("Unknown device type", nameof(model.DeviceType))
            };

            entity.UserGuid = model.UserGuid;
            entity.DeviceName = model.DeviceName;
            entity.Serial = model.Serial;
            entity.IsManuallyOperated = model.IsManuallyOperated;
            entity.EnergyConsumption = model.EnergyConsumption;
            entity.OnTime = model.OnTime;
            entity.OffTime = model.OffTime;
            entity.IsOn = model.IsOn;

            if (entity is HeatPumpEntity heatPump)
            {
                heatPump.Degree = model.Degree;
            }

            return entity;
        }

    }
}
