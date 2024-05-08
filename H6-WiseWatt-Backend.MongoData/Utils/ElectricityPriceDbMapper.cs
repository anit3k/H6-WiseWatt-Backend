using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.MongoData.Models;

namespace H6_WiseWatt_Backend.MongoData.Utils
{
    public class ElectricityPriceDbMapper
    {
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
    }
}
