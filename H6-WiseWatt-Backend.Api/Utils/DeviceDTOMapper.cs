using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Api.Utils
{
    /// <summary>
    /// Utility designed to map between DeviceDTO and IoTDeviceBaseEntity objects. 
    /// This class helps translate data between Data Transfer Objects (DTOs) 
    /// and domain entities, which represent the core business objects within the backend.
    /// </summary>
    public class DeviceDTOMapper
    {
        #region Internal Methods
        /// <summary>
        /// Converts an IoTDeviceBaseEntity to a DeviceDTO. It populates the DTO with the appropriate attributes from the entity, 
        /// including user identifiers, device characteristics, and operational details. 
        /// It also handles special cases based on specific device types (e.g., setting the Degree for heat pumps).
        /// </summary>
        /// <param name="entity">Current IoT Device</param>
        /// <returns>DeviceDTO</returns>
        internal DeviceDTO MapToDeviceDto(IoTDeviceBaseEntity entity)
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

        /// <summary>
        /// Converts a DeviceDTO to an IoTDeviceBaseEntity. 
        /// This method creates an instance of the appropriate device type 
        /// based on the DeviceType attribute and then populates the entity with information from the DTO. 
        /// It throws an ArgumentException if the device type is unknown.
        /// </summary>
        /// <param name="model">DeviceDTO<param>
        /// <returns>IoTDeviceBaseEntity</returns>
        internal IoTDeviceBaseEntity MapToDeviceEntity(DeviceDTO model)
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
        #endregion
    }
}
