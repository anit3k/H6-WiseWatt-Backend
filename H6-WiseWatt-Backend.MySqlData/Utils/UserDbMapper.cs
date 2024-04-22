using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.MySqlData.Models;

namespace H6_WiseWatt_Backend.MySqlData.Utils
{
    /// <summary>
    /// Utility for mapping user entities between the domain logic (UserEntity) and the database representation (UserDbModel) specific to MySQL.
    /// </summary>
    public class UserDbMapper
    {
        #region Internal Methods
        /// <summary>
        /// Responsible for mapping a UserEntity object along with password hash and salt to a corresponding UserDbModel object.
        /// </summary>
        internal UserDbModel MapToUserDbModel(UserEntity user, string passwordHash, string salt)
        {
            return new UserDbModel
            {
                UserGuid = user.UserGuid,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                PasswordHash = passwordHash,
                Salt = salt
            };
        }

        /// <summary>
        /// Responsible for mapping a UserDbModel object from the database to a corresponding UserEntity object.
        /// </summary>
        internal UserEntity MapToUserEntity(UserDbModel user, string password = null)
        {
            return new UserEntity()
            {
                UserGuid = user.UserGuid,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Password = password,
            };
        }
        #endregion
    }
}
