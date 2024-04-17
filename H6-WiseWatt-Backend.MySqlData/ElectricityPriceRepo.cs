using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class ElectricityPriceRepo : IElectricityPriceRepo
    {
        private readonly MySqlDbContext _dbContext;

        public ElectricityPriceRepo(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ElectricityPriceEntity>> GetAllPrices()
        {
            var priceModels = await _dbContext.ElectricityPrices.ToListAsync();
            if (priceModels == null || priceModels.Count == 0)
            {
                return new List<ElectricityPriceEntity>();
            }
            var result = priceModels.Select( pm => MapToPriceEntity(pm)).ToList();
            return result;
        }       

        public Task<ElectricityPriceEntity> GetPrice(DateTime timeStamp)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatePrices(List<ElectricityPriceEntity> priceUpdate)
        {
            var existingTimestamps = await _dbContext.ElectricityPrices
                                              .Select(e => e.TimeStamp)
                                              .ToListAsync();

            var newEntries = priceUpdate.Where(pu => !existingTimestamps.Contains(pu.TimeStamp))
                                        .Select(pu => MapFromPriceEntity(pu))
                                        .ToList();

            if (newEntries.Any())
            {
                _dbContext.ElectricityPrices.AddRange(newEntries);
                await _dbContext.SaveChangesAsync();
            }
        }
        private ElectricityPriceEntity MapToPriceEntity(ElectricityPriceDbModel pm)
        {
            return new ElectricityPriceEntity
            {
                TimeStamp = pm.TimeStamp,
                PricePerKwh = pm.PricePerKwh,
                TransportAndDuties = pm.TransportAndDuties,
                TotalPrice = pm.TotalPrice,
            };
        }
        private ElectricityPriceDbModel MapFromPriceEntity(ElectricityPriceEntity pu)
        {
            return new ElectricityPriceDbModel
            {
                TimeStamp = pu.TimeStamp,
                PricePerKwh = pu.PricePerKwh,
                TransportAndDuties = pu.TransportAndDuties,
                TotalPrice = pu.TotalPrice,
            };
        }
    }
}
