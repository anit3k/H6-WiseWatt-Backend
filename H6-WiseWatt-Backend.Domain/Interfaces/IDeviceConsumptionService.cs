using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IDeviceConsumptionService
    {
        ConsumptionStatisticsEntity CalculateStatistics(List<IoTDeviceBaseEntity> devices);
    }
}
