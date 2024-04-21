using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.MySqlData.Models;

namespace H6_WiseWatt_Backend.MySqlData.Utils
{
    public class UserDbMapper
    {
        public UserDbModel MapToUserDbModel(UserEntity user, string passwordHash, string salt)
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

        public UserEntity MapToUserEntity(UserDbModel user, string password = null)
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
    }
}
