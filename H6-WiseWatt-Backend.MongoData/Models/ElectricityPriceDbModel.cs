using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace H6_WiseWatt_Backend.MongoData.Models
{
    public class ElectricityPriceDbModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double PricePerKwh { get; set; }
        public double TransportAndDuties { get; set; }
        public double TotalPrice { get; set; }
    }
}
