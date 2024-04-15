using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class UserDeviceRepo : IDeviceRepo
    {
        private readonly MySqlDbContext _dbContext;

        public UserDeviceRepo(MySqlDbContext dbContext)
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
            return model.DeviceType switch
            {
                "Dishwasher" => new DishwasherEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime,
                },
                "Dryer" => new DryerEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime,
                },
                "CarCharger" => new ElectricCarChargerEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime
                },
                "HeatPump" => new HeatPumpEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime,
                    Degree = model.Degree,
                },
                "WashingMachine" => new WashingMachineEntity
                {
                    UserGuid = model.UserGuid,
                    DeviceName = model.DeviceName,
                    Serial = model.Serial,
                    IsOn = model.IsOn,
                    EnergyConsumption = model.EnergyConsumption,
                    OnTime = model.OnTime,
                    OffTime = model.OffTime,
                },
                _ => throw new ArgumentException("Unknown device type", nameof(model.DeviceType))
            };
        }
    }
}
