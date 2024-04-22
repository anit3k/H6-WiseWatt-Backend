using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.MySqlData.Models;

namespace H6_WiseWatt_Backend.MySqlData.Utils
{
    /// <summary>
    /// Provides methods for mapping between ElectricityPriceEntity objects and ElectricityPriceDbModel objects. 
    /// These mappings facilitate the transfer of data between the domain entities and the database models.
    /// </summary>
    public class ElectricityPriceDbMapper
    {
        #region Internal Methods
        /// <summary>
        /// Maps an ElectricityPriceDbModel object to an ElectricityPriceEntity object.
        /// </summary>
        /// <param name="pm">ElectricityPriceDbModel</param>
        /// <returns>ElectricityPriceEntity</returns>
        internal ElectricityPriceEntity MapToPriceEntity(ElectricityPriceDbModel pm)
        {
            return new ElectricityPriceEntity
            {
                TimeStamp = pm.TimeStamp,
                PricePerKwh = pm.PricePerKwh,
                TransportAndDuties = pm.TransportAndDuties,
                TotalPrice = pm.TotalPrice,
            };
        }

        /// <summary>
        /// Maps an ElectricityPriceEntity object to an ElectricityPriceDbModel object.
        /// </summary>
        /// <param name="pu">ElectricityPriceEntity</param>
        /// <returns>ElectricityPriceDbModel</returns>
        internal ElectricityPriceDbModel MapFromPriceEntity(ElectricityPriceEntity pu)
        {
            return new ElectricityPriceDbModel
            {
                TimeStamp = pu.TimeStamp,
                PricePerKwh = pu.PricePerKwh,
                TransportAndDuties = pu.TransportAndDuties,
                TotalPrice = pu.TotalPrice,
            };
        }
        #endregion
    }
}
