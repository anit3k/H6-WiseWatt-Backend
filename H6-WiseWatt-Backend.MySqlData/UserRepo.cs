using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Utils;
using H6_WiseWatt_Backend.Security.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class UserRepo : IUserRepo
    {
        private readonly MySqlDbContext _dbContext;
        private readonly UserDbMapper _userMapper;
        private readonly IPasswordHasher _passwordService;

        public UserRepo(MySqlDbContext dbContext, UserDbMapper userMapper, IPasswordHasher passwordService)
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

        public async Task<bool> ValidateUserEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<UserEntity?> GetUser(UserEntity user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (dbUser == null) { return null; }
            return _userMapper.MapToUserEntity(dbUser);
        }

        public async Task<UserEntity> UpdateUser(UserEntity user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserGuid == user.UserGuid);

            var salt = _passwordService.GenerateSalt();
            var passwordHash = _passwordService.HashPasswordWithSalt(user.Password, salt);
            dbUser.Firstname = user.Firstname;
            dbUser.Lastname = user.Lastname;
            dbUser.Email = user.Email;
            dbUser.PasswordHash = passwordHash;
            dbUser.Salt = salt;

            await _dbContext.SaveChangesAsync();
            return _userMapper.MapToUserEntity(dbUser, user.Password);
        }
    }
}
