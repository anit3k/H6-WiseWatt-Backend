using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IElectricityPriceRepo
    {
        public Task<ElectricityPriceEntity> GetPrice(DateTime timeStamp);
        public Task<List<ElectricityPriceEntity>> GetAllPrices();
        public Task UpdatePrices(List<ElectricityPriceEntity> priceUpdate);
    }
}
