using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Api.Utils
{
    /// <summary>
    /// Utility designed to map between UserDTO and UserEntity objects. 
    /// This class plays a crucial role in translating data between Data Transfer Objects (DTOs) and domain entities, 
    /// enabling seamless data exchange between frontend applications and backend services in user-related operations.
    /// </summary>
    public class UserDTOMapper
    {
        #region Internal Methods
        /// <summary>
        /// Converts a UserEntity object to a UserDTO. 
        /// It extracts the necessary attributes from the domain entity 
        /// and creates a DTO with these values, ensuring a consistent structure for data transfer.
        /// </summary>
        /// <param name="userEntity">UserEntity</param>
        /// <returns>UserDTO</returns>
        internal UserDTO MapToUserDto(UserEntity userEntity)
        {
            return new UserDTO
            {
                Email = userEntity.Email,
                Firstname = userEntity.Firstname,
                Lastname = userEntity.Lastname,
                Password = userEntity.Password,
            };
        }

        /// <summary>
        /// Converts a UserDTO object to a UserEntity. 
        /// It creates a new domain entity with the given user data, 
        /// including the UserGuid, which is provided as an optional parameter.
        /// </summary>
        /// <param name="user">UserDTO</param>
        /// <param name="userGuid">User unique id, used when updating current user</param>
        /// <returns>UserEntity</returns>
        internal UserEntity MapToUserEntity(UserDTO user, string userGuid = null)
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
        #endregion
    }
}
