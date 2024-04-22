using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace H6_WiseWatt_Backend.MySqlData
{
    /// <summary>
    /// Encapsulates the logic for querying and updating electricity price data in the MySQL database, 
    /// leveraging the ElectricityPriceDbMapper for mapping between domain entities and database models.
    /// </summary>
    public class ElectricityPriceRepo : IElectricityPriceRepo
    {
        #region fields
        private readonly MySqlDbContext _dbContext;
        private readonly ElectricityPriceDbMapper _electricityPriceDbMapper;
        #endregion

        #region Constructor
        public ElectricityPriceRepo(MySqlDbContext dbContext, ElectricityPriceDbMapper electricityPriceDbMapper)
        {
            _dbContext = dbContext;
            _electricityPriceDbMapper = electricityPriceDbMapper;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Retrieves all electricity price records from the database.
        /// </summary>
        /// <returns>A List of ElectricityPriceEntity</returns>
        public async Task<List<ElectricityPriceEntity>> GetAllPrices()
        {
            var priceModels = await _dbContext.ElectricityPrices.ToListAsync();
            if (priceModels == null || priceModels.Count == 0)
            {
                return new List<ElectricityPriceEntity>();
            }
            var result = priceModels.Select(pm => _electricityPriceDbMapper.MapToPriceEntity(pm)).ToList();
            return result;
        }

        /// <summary>
        /// Retrieves an electricity price record from the database based on the specified time stamp
        /// </summary>
        /// <param name="timeStamp">DateTime</param>
        /// <returns>ElectricityPriceEntity</returns>
        public async Task<ElectricityPriceEntity> GetPrice(DateTime timeStamp)
        {
            var price = await _dbContext.ElectricityPrices.Where(ts => ts.TimeStamp == timeStamp).FirstOrDefaultAsync();
            return _electricityPriceDbMapper.MapToPriceEntity(price);
        }

        /// <summary>
        /// Retrieves existing timestamps of electricity price records from the database and
        /// compares the timestamps of the provided price update with the existing timestamps to identify new entries,
        /// and adds them to the database
        /// </summary>
        /// <param name="priceUpdate"></param>
        /// <returns></returns>
        public async Task UpdatePrices(List<ElectricityPriceEntity> priceUpdate)
        {
            var existingTimestamps = await _dbContext.ElectricityPrices
                                              .Select(e => e.TimeStamp)
                                              .ToListAsync();

            var newEntries = priceUpdate.Where(pu => !existingTimestamps.Contains(pu.TimeStamp))
                                        .Select(pu => _electricityPriceDbMapper.MapFromPriceEntity(pu))
                                        .ToList();

            if (newEntries.Any())
            {
                _dbContext.ElectricityPrices.AddRange(newEntries);
                await _dbContext.SaveChangesAsync();
            }
        }        
        #endregion
    }
}
