using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Api.Utils
{
    public class UserDTOMapper
    {
        public UserDTO MapToUserDto(UserEntity userEntity)
        {
            return new UserDTO
            {
                Email = userEntity.Email,
                Firstname = userEntity.Firstname,
                Lastname = userEntity.Lastname,
                Password = userEntity.Password,
            };
        }

        public UserEntity MapToUserEntity(UserDTO user, string userGuid = null)
        {
            return new UserEntity
            {
                UserGuid = userGuid,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Password = user.Password,
            };
        }
    }
}
