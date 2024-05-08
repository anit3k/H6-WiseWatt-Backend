using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace H6_WiseWatt_Backend.MongoData.Models
{
    /// <summary>
    /// Data model for storing device-related information in a MySQL database. It corresponds to the Device entity class used in the application's domain logic.
    /// </summary>
    public class DeviceDbModel
    {
        #region Properties
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserGuid { get; set; }

        public string DeviceType { get; set; }

        public string DeviceName { get; set; }

        public string Serial { get; set; }

        public bool IsManuallyOperated { get; set; }
        public bool IsOn { get; set; }

        public double EnergyConsumption { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan OnTime { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan OffTime { get; set; }
        public int Degree { get; set; }
        #endregion
    }
}
