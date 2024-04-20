using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Api.Utils
{
    public class DeviceDTOMapper
    {
        public DeviceDTO MapToDeviceDto(IoTDeviceBaseEntity entity)
        {
            var model = new DeviceDTO
            {
                UserGuid = entity.UserGuid,
                DeviceType = entity.DeviceType.ToString(),
                DeviceName = entity.DeviceName,
                Serial = entity.Serial,
                IsManuallyOperated = entity.IsManuallyOperated,
                EnergyConsumption = entity.EnergyConsumption,
                OnTime = entity.OnTime,
                OffTime = entity.OffTime,
                IsOn = entity.IsOn,
                Degree = null,
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

        public IoTDeviceBaseEntity MapToDeviceEntity(DeviceDTO model)
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


            if (entity is HeatPumpEntity heatPump && model.Degree.HasValue)
            {
                heatPump.Degree = model.Degree.Value;
            }

            return entity;
        }
    }
}
