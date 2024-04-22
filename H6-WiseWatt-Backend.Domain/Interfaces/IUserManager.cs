using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for user management operations within the domain layer. 
    /// It specifies methods for creating, retrieving, updating, and deleting user entities asynchronously, along with validating user email addresses.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Asynchronously validates whether a user with the specified email address exists.
        /// </summary>
        /// <param name="email">Current users e-mail</param>
        /// <returns>true/false</returns>
        Task<bool> ValidateUserByEmail(string email);

        /// <summary>
        /// Asynchronously creates a new user based on the provided user entity.
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>true/false</returns>
        Task<bool> CreateNewUser(UserEntity user);

        /// <summary>
        /// Asynchronously retrieves a user entity based on the provided user entity.
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>UserEntity</returns>
        Task<UserEntity> GetUser(UserEntity user);

        /// <summary>
        /// Asynchronously updates an existing user entity with the provided user entity's data.
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>Updated UserEntity</returns>
        Task<UserEntity> UpdateCurrentUser(UserEntity user);

        /// <summary>
        /// Asynchronously deletes a user entity with the specified user GUID.
        /// </summary>
        /// <param name="userGuide">Current users unique identifier</param>
        /// <returns>true/false</returns>
        Task<bool> DeleteCurrentUser(string userGuide);
    }
}
