using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.MySqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace H6_WiseWatt_Backend.MySqlData
{
    public class UserMySqlRepo : IUserRepo
    {
        private readonly MySqlDbContext _dbContext;

        public UserMySqlRepo(MySqlDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateNewUser(UserEntity user)
        {
            _dbContext.Users.Add(new UserDbModel { Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email, Password = user.Password });
            var result = await _dbContext.SaveChangesAsync();
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ValidateUsernameEmail(UserEntity user)
        {
            return await _dbContext.Users.AnyAsync(u => u.Firstname == user.Firstname || u.Email == user.Email);
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
            };
        }
    }
}
