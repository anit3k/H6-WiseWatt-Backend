using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Serilog;

namespace H6_WiseWatt_Backend.Domain.Services
{
    /// <summary>
    /// Responsible for managing user-related operations within the domain layer. 
    /// It implements the IUserManager interface, providing methods for creating, retrieving, updating, and deleting user information. 
    /// Additionally, it handles associated operations like creating default devices when a new user is registered.
    /// </summary>
    public class UserManager : IUserManager
    {
        #region fields
        private readonly IUserRepo _userRepo;
        private readonly IDeviceRepo _deviceRepo;
        private readonly IDeviceFactory _deviceFactory;
        #endregion

        #region Constructor
        public UserManager(IUserRepo userRepo, IDeviceRepo deviceRepo, IDeviceFactory deviceFactory)
        {
            _userRepo = userRepo;
            _deviceRepo = deviceRepo;
            _deviceFactory = deviceFactory;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates a new user and assigns a unique GUID to the user. 
        /// It also creates a set of default IoT devices for the new user using IDeviceFactory. 
        /// If an error occurs, it logs the error and re-throws the exception.
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>true/false</returns>
        public async Task<bool> CreateNewUser(UserEntity user)
        {
            try
            {
                user.UserGuid = Guid.NewGuid().ToString();
                var userGuid = await _userRepo.CreateNewUser(user);
                var devices = _deviceFactory.CreateDefaultDevices();

                foreach (var device in devices)
                {
                    device.UserGuid = userGuid;
                    await _deviceRepo.CreateDevice(device);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Error occur while creating new user: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        ///  Deletes a user based on their user GUID by calling the repository method to remove the user from the data store.
        /// </summary>
        /// <param name="userGuide">Users unique identifier</param>
        /// <returns>true/false</returns>
        public async Task<bool> DeleteCurrentUser(string userGuide)
        {
            return await _userRepo.DeleteUser(userGuide);
        }

        /// <summary>
        /// Retrieves user information based on a UserEntity object. 
        /// This method is asynchronous and relies on the repository to fetch user details.
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>A user entity</returns>
        public async Task<UserEntity> GetUser(UserEntity user)
        {
            return await _userRepo.GetUser(user);
        }

        /// <summary>
        /// Updates user information by sending the updated UserEntity to the repository. The method returns the updated user.
        /// </summary>
        /// <param name="user">Current user</param>
        /// <returns>A user entity</returns>
        public async Task<UserEntity> UpdateCurrentUser(UserEntity user)
        {
            return await _userRepo.UpdateUser(user);
        }

        /// <summary>
        /// Validates whether a user exists based on their email address. 
        /// This method helps in scenarios like user registration to check for existing users.
        /// </summary>
        /// <param name="email">Email of a possible new user</param>
        /// <returns>true/false</returns>
        public async Task<bool> ValidateUserByEmail(string email)
        {
            return await _userRepo.ValidateUserEmail(email);
        }
        #endregion
    }
}
