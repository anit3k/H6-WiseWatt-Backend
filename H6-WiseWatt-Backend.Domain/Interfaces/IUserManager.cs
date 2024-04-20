using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IUserManager
    {
        Task<bool> ValidateUserByEmail(string email);
        Task<bool> CreateNewUser(UserEntity user);
        Task<UserEntity> GetUser(UserEntity user);
    }
}
