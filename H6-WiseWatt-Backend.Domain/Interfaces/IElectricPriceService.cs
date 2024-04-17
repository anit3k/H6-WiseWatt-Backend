using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IElectricPriceService
    {
        public Task<List<ElectricityPriceEntity>> GetElectricityPricesAsync();
    }
}
