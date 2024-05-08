using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace H6_WiseWatt_Backend.MongoData.Models
{
    /// <summary>
    /// Data model for storing user-related information in a MySQL database. It corresponds to the UserEntity class used in the application's domain logic.
    /// </summary>
    public class UserDbModel
    {
        #region Properties
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserGuid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        #endregion
    }
}
}
