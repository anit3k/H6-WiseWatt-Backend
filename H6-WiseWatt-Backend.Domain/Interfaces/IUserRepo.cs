using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    public interface IUserRepo
    {
        Task<string> CreateNewUser(UserEntity user);
        Task<UserEntity> GetUser(UserEntity user);
        Task<bool> ValidateUserEmail(string email);
        Task<UserEntity> UpdateUser(UserEntity user);
        Task<bool> DeleteUser(string userGuid);
    }
}
