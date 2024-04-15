using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class UserRepo : IUserRepo
    {
        private readonly MySqlDbContext _dbContext;

        public UserRepo(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateNewUser(UserEntity user)
        {
            var dbUser = new UserDbModel { Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email, Password = user.Password, UserGuid = Guid.NewGuid().ToString() };
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
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
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
