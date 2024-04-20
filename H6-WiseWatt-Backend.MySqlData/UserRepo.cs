using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Models;
using H6_WiseWatt_Backend.Security.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class UserRepo : IUserRepo
    {
        private readonly MySqlDbContext _dbContext;
        private readonly IPasswordHasher _passwordService;

        public UserRepo(MySqlDbContext dbContext, IPasswordHasher passwordService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
        }

        public async Task<string> CreateNewUser(UserEntity user)
        {
            var salt = _passwordService.GenerateSalt();
            var passwordHash = _passwordService.HashPasswordWithSalt(user.Password, salt);
            var dbUser = new UserDbModel { Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email, PasswordHash = passwordHash, Salt = salt, UserGuid = Guid.NewGuid().ToString() };
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

        public async Task<bool> ValidateUserEmail(UserEntity user)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == user.Email);
        }

        public async Task<UserEntity?> GetUser(UserEntity user)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (dbUser == null) { return null; }
            return new UserEntity()
            {
                Firstname = dbUser.Firstname,
                Lastname = dbUser.Lastname,
                Email = dbUser.Email,
                UserGuid = dbUser.UserGuid,
            };
        }
    }
}
