using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IUserManager
    {
        Task<bool> ValidateUserByEmail(string email);
        Task<bool> CreateNewUser(UserEntity user);
        Task<UserEntity> GetUser(UserEntity user);
        Task<UserEntity> UpdateCurrentUser(UserEntity user);
        Task<bool> DeleteCurrentUser(string userGuide);
    }
}
