using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.MySqlData.Models;

namespace H6_WiseWatt_Backend.MySqlData.Utils
{
    /// <summary>
    /// Provides methods for mapping between IoTDeviceBaseEntity objects and DeviceDbModel objects. 
    /// These mappings facilitate the transfer of data between the domain entities and the database models.
    /// </summary>
    public class DeviceDbMapper
    {
        #region Internal Methods
        /// <summary>
        /// Maps an IoTDeviceBaseEntity object to a DeviceDbModel object.
        /// </summary>
        /// <param name="entity">IoTDeviceBaseEntity</param>
        /// <returns>DeviceDbModel</returns>
        internal DeviceDbModel MapToDeviceDbModel(IoTDeviceBaseEntity entity)
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

        /// <summary>
        /// Performs mapping, converting a DeviceDbModel object to an IoTDeviceBaseEntity object.
        /// </summary>
        /// <param name="model">DeviceDbModel</param>
        /// <returns>IoTDeviceBaseEntityreturns>
        internal IoTDeviceBaseEntity MapToDeviceEntity(DeviceDbModel model)
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
        #endregion
    }
}
