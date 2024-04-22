using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    /// <summary>
    /// defines the contract for repository operations related to user entities within the domain layer. 
    /// It outlines asynchronous methods for creating, retrieving, updating, and deleting user entities, as well as validating user email addresses.
    /// </summary>
    public interface IUserRepo
    {
        /// <summary>
        /// Asynchronously creates a new user entity based on the provided user entity and returns the unique user id of the created user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> CreateNewUser(UserEntity user);

        /// <summary>
        /// Asynchronously retrieves a user entity based on the provided user entity.
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>UserEntity</returns>
        Task<UserEntity> GetUser(UserEntity user);

        /// <summary>
        /// Asynchronously validates whether a user with the specified email address exists.
        /// </summary>
        /// <param name="email">Current users e-mail</param>
        /// <returns>true/false</returns>
        Task<bool> ValidateUserEmail(string email);

        /// <summary>
        ///  Asynchronously updates an existing user entity with the provided user entity's data.
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>UserEntity</returns>
        Task<UserEntity> UpdateUser(UserEntity user);

        /// <summary>
        /// Asynchronously deletes a user entity with the specified unique user id.
        /// </summary>
        /// <param name="userGuid">Users unique identifier</param>
        /// <returns>true/false</returns>
        Task<bool> DeleteUser(string userGuid);
    }
}
