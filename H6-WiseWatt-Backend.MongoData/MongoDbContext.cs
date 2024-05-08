using H6_WiseWatt_Backend.MongoData.Models;
using MongoDB.Driver;

namespace H6_WiseWatt_Backend.MongoData
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext()
        {
            var client = new MongoClient("mongodb+srv://admin:Kode1234!@cluster0.juq1azd.mongodb.net/");
            _database = client.GetDatabase("WiseWatt");
        }

        public void ResetDb()
        {
            _database.DropCollection("Devices");
            _database.DropCollection("Users");
            _database.DropCollection("ElectricityPrices");
        }

        public IMongoCollection<DeviceDbModel> Devices => _database.GetCollection<DeviceDbModel>("Devices");
        public IMongoCollection<UserDbModel> Users => _database.GetCollection<UserDbModel>("Users");
        public IMongoCollection<ElectricityPriceDbModel> ElectricityPrices => _database.GetCollection<ElectricityPriceDbModel>("ElectricityPrices");
    }
}
