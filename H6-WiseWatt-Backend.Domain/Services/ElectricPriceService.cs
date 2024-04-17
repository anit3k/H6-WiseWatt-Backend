using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    public class ElectricPriceService : IElectricPriceService
    {
        private readonly IElectricityPriceRepo _priceRepo;

        public ElectricPriceService(IElectricityPriceRepo priceRepo )
        {
            _priceRepo = priceRepo;
        }
        public async Task<List<ElectricityPriceEntity>> GetElectricityPricesAsync()
        {
            var allPrices = await _priceRepo.GetAllPrices();

            if ( allPrices == null )
            {
                // do logic
            }

            var result  = Map
            return allPrices;
        }
    }
}
