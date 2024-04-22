using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Utils;
using H6_WiseWatt_Backend.Security.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    /// <summary>
    /// Responsible for handling user-related CRUD operations in the MySQL database
    /// </summary>
    public class UserRepo : IUserRepo
    {
        #region fields
        private readonly MySqlDbContext _dbContext;
        private readonly UserDbMapper _userMapper;
        private readonly IPasswordHasher _passwordService;
        #endregion

        #region Constructor
        public UserRepo(MySqlDbContext dbContext, UserDbMapper userMapper, IPasswordHasher passwordService)
        {
            _dbContext = dbContext;
            _userMapper = userMapper;
            _passwordService = passwordService;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// This method generates a unique GUID for the user, hashes the user's password with a generated salt, 
        /// and then maps the user entity to the database model. It adds the user to the context and saves the changes.
        /// </summary>
        /// <param name="user">UserEntity user - The user entity containing the information needed to create a new user.</param>
        /// <returns>string - The unique identifier (UserGuid) for the created user. Returns an empty string if the operation fails.</returns>
        public async Task<string> CreateNewUser(UserEntity user)
        {
            var salt = _passwordService.GenerateSalt();
            var passwordHash = _passwordService.HashPasswordWithSalt(user.Password, salt);
            user.UserGuid = Guid.NewGuid().ToString();            
            var dbUser = _userMapper.MapToUserDbModel(user, passwordHash, salt);
            _dbContext.Users.Add(dbUser);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 1)
            {
                return dbUser.UserGuid;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// This method checks if any user in the database has the given email. It's typically used to ensure email uniqueness before creating or updating a user.
        /// </summary>
        /// <param name="email">string email - The email to validate.</param>
        /// <returns>bool - true if the email is found, otherwise false.</returns>
        public async Task<bool> ValidateUserEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// This method looks for a user in the database using the user GUID or email. If found, it maps the database user model to the user entity and returns it.
        /// </summary>
        /// <param name="user">UserEntity user - A user entity with the user GUID or email to search for.</param>
        /// <returns>UserEntity? - The user entity if found, otherwise null.</returns>
        public async Task<UserEntity?> GetUser(UserEntity user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserGuid == user.UserGuid || u.Email == user.Email);
            if (dbUser == null) { return null; }
            return _userMapper.MapToUserEntity(dbUser);
        }

        /// <summary>
        /// This method retrieves the user from the database using the user GUID, then updates various fields such as first name, last name, email, and password. 
        /// It saves the changes to the database and returns the updated user entity.
        /// </summary>
        /// <param name="user">UserEntity user - The user entity with updated information.</param>
        /// <returns>UserEntity - The updated user entity.</returns>
        public async Task<UserEntity> UpdateUser(UserEntity user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserGuid == user.UserGuid);

            if (user.Firstname != null && dbUser.Firstname != user.Firstname)
            {
                dbUser.Firstname = user.Firstname;
            }

            if (user.Lastname != null && dbUser.Lastname != user.Lastname)
            {
                dbUser.Lastname = user.Lastname;
            }

            if (user.Email != null && dbUser.Email != user.Email)
            {
                dbUser.Email = user.Email;
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                var salt = _passwordService.GenerateSalt();
                var passwordHash = _passwordService.HashPasswordWithSalt(user.Password, salt);
                dbUser.PasswordHash = passwordHash;
                dbUser.Salt = salt;
            }

            await _dbContext.SaveChangesAsync();
            return _userMapper.MapToUserEntity(dbUser, user.Password);
        }

        /// <summary>
        /// This method finds the user in the database using the user GUID and removes the corresponding record. 
        /// It commits the changes and returns a boolean indicating success or failure.
        /// </summary>
        /// <param name="userGuid">string userGuid - The unique identifier of the user to delete.</param>
        /// <returns>bool - true if the user is deleted, otherwise false.</returns>
        public async Task<bool> DeleteUser(string userGuid)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserGuid == userGuid);
            if (dbUser != null) 
            {
                _dbContext.Users.Remove(dbUser);
                var result = await _dbContext.SaveChangesAsync();
                if (result == 1)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
