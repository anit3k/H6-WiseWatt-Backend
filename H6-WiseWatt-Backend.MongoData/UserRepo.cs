using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MongoData.Models;
using H6_WiseWatt_Backend.MongoData.Utils;
using H6_WiseWatt_Backend.Security.Interfaces;
using MongoDB.Driver;

namespace H6_WiseWatt_Backend.MongoData
{
    public class UserRepo : IUserRepo
    {
        private readonly MongoDbContext _dbContext;
        private readonly UserDbMapper _userMapper;
        private readonly IPasswordHasher _passwordService;

        public UserRepo(MongoDbContext dbContext, UserDbMapper userMapper, IPasswordHasher passwordService)
        {
            _dbContext = dbContext;
            _userMapper = userMapper;
            _passwordService = passwordService;
        }


        public async Task<string> CreateNewUser(UserEntity user)
        {
            var salt = _passwordService.GenerateSalt();
            var passwordHash = _passwordService.HashPasswordWithSalt(user.Password, salt);
            user.UserGuid = Guid.NewGuid().ToString();
            var dbUser = _userMapper.MapToUserDbModel(user, passwordHash, salt);
            await _dbContext.Users.InsertOneAsync(dbUser);
            return dbUser.UserGuid;
        }

        public async Task<bool> ValidateUserEmail(string email)
        {
            return await _dbContext.Users.Find(u => u.Email == email).AnyAsync();
        }

        public async Task<UserEntity?> GetUser(UserEntity user)
        {
            var filter = Builders<UserDbModel>.Filter.Or(
                Builders<UserDbModel>.Filter.Eq("UserGuid", user.UserGuid),
                Builders<UserDbModel>.Filter.Eq("Email", user.Email)
            );
            var dbUser = await _dbContext.Users.Find(filter).FirstOrDefaultAsync();
            return dbUser != null ? _userMapper.MapToUserEntity(dbUser) : null;
        }

        public async Task<UserEntity> UpdateUser(UserEntity user)
        {
            var filter = Builders<UserDbModel>.Filter.Eq("UserGuid", user.UserGuid);
            var dbUser = await _dbContext.Users.Find(filter).FirstOrDefaultAsync();

            if (dbUser != null)
            {
                if (!string.IsNullOrEmpty(user.Firstname))
                    dbUser.Firstname = user.Firstname;
                if (!string.IsNullOrEmpty(user.Lastname))
                    dbUser.Lastname = user.Lastname;
                if (!string.IsNullOrEmpty(user.Email))
                    dbUser.Email = user.Email;
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var salt = _passwordService.GenerateSalt();
                    var passwordHash = _passwordService.HashPasswordWithSalt(user.Password, salt);
                    dbUser.PasswordHash = passwordHash;
                    dbUser.Salt = salt;
                }

                await _dbContext.Users.ReplaceOneAsync(filter, dbUser);
                return _userMapper.MapToUserEntity(dbUser, user.Password);
            }
            return null;
        }

        public async Task<bool> DeleteUser(string userGuid)
        {
            var filter = Builders<UserDbModel>.Filter.Eq("UserGuid", userGuid);
            var result = await _dbContext.Users.DeleteOneAsync(filter);
            return result.DeletedCount == 1;
        }
    }
}
