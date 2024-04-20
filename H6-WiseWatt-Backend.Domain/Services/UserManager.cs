using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Serilog;

namespace H6_WiseWatt_Backend.Domain.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepo _userRepo;
        private readonly IDeviceRepo _deviceRepo;
        private readonly IDeviceFactory _deviceFactory;
        public UserManager(IUserRepo userRepo, IDeviceRepo deviceRepo, IDeviceFactory deviceFactory)
        {
            _userRepo = userRepo;
            _deviceRepo = deviceRepo;
            _deviceFactory = deviceFactory;
        }
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

        public Task<UserEntity> GetUser(UserEntity user)
        {
            return _userRepo.GetUser(user);
        }

        public async Task<bool> ValidateUserByEmail(string email)
        {
            return await _userRepo.ValidateUserEmail(email);
        }
    }
}
