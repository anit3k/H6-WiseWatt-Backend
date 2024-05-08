using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MongoData.Models;
using H6_WiseWatt_Backend.MongoData.Utils;
using MongoDB.Driver;

namespace H6_WiseWatt_Backend.MongoData
{
    public class ElectricityPriceRepo : IElectricityPriceRepo
    {
        private readonly MongoDbContext _dbContext;
        private readonly ElectricityPriceDbMapper _electricityPriceDbMapper;

        public ElectricityPriceRepo(MongoDbContext dbContext, ElectricityPriceDbMapper electricityPriceDbMapper)
        {
            _dbContext = dbContext;
            _electricityPriceDbMapper = electricityPriceDbMapper;
        }

        public async Task<List<ElectricityPriceEntity>> GetAllPrices()
        {
            var priceModels = await _dbContext.ElectricityPrices.Find(_ => true).ToListAsync();
            return priceModels.Select(pm => _electricityPriceDbMapper.MapToPriceEntity(pm)).ToList();
        }

        public async Task<ElectricityPriceEntity> GetPrice(DateTime timeStamp)
        {
            var filter = Builders<ElectricityPriceDbModel>.Filter.Eq("TimeStamp", timeStamp);
            var priceModel = await _dbContext.ElectricityPrices.Find(filter).FirstOrDefaultAsync();
            return priceModel != null ? _electricityPriceDbMapper.MapToPriceEntity(priceModel) : null;
        }

        public async Task UpdatePrices(List<ElectricityPriceEntity> priceUpdate)
        {
            var existingTimestamps = (await _dbContext.ElectricityPrices
                                              .Find(_ => true)
                                              .ToListAsync())
                                              .Select(e => e.TimeStamp)
                                              .ToHashSet();

            var newEntries = priceUpdate.Where(pu => !existingTimestamps.Contains(pu.TimeStamp))
                                        .Select(pu => _electricityPriceDbMapper.MapFromPriceEntity(pu))
                                        .ToList();

            if (newEntries.Any())
            {
                await _dbContext.ElectricityPrices.InsertManyAsync(newEntries);
            }
        }
    }
}
