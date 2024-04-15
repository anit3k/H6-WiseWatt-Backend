using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Factories;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IDeviceRepo _deviceRepo;
        private readonly IIoTDeviceFactory _deviceFactory;

        public UserController(IUserRepo userRepo, IDeviceRepo deviceRepo, IIoTDeviceFactory deviceFactory)
        {
            _userRepo = userRepo;
            _deviceRepo = deviceRepo;
            _deviceFactory = deviceFactory;
        }

        [HttpPost]
        [Route("api/user/register")]
        public async Task<IActionResult> RegisterUser(UserDto user)
        {
            try
            {
                if (IsNotValid(user))
                {
                    return BadRequest("Invalid User Information");
                }

                if (await DoUserExist(user))
                {
                    return BadRequest("User already exist");
                }

                string userGuid = await AddNewUserToRepo(user);
                if (IsUserGuidValid(userGuid))
                {
                    await CreateDefaultDevicesToNewUser(userGuid);
                    return Ok("User has been created");
                }
                else
                {
                    return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error has occurred with error message: {ex.Message}");
                return StatusCode(statusCode: 500, "Something went wrong please contact your administrator");
            }
        }

        private bool IsNotValid(UserDto user)
        {
            return user == null || string.IsNullOrWhiteSpace(user.Firstname) && string.IsNullOrWhiteSpace(user.Password) && string.IsNullOrWhiteSpace(user.Email);
        }

        private async Task<bool> DoUserExist(UserDto user)
        {
            var result = await _userRepo.ValidateUserEmail(new UserEntity { Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email });
            return result;
        }

        private async Task<string> AddNewUserToRepo(UserDto user)
        {
            return await _userRepo.CreateNewUser(new UserEntity { Password = user.Password, Firstname = user.Firstname, Lastname = user.Lastname, Email = user.Email, UserGuid = "669cadd0-70cf-43a1-9a9d-426212185666" });
        }

        private bool IsUserGuidValid(string userGuid)
        {
            return userGuid != string.Empty;
        }        

        private async Task CreateDefaultDevicesToNewUser(string userGuid)
        {
            var devices = _deviceFactory.CreateDefaultDevices();

            foreach (var device in devices)
            {
                device.UserGuid = userGuid;
                await _deviceRepo.CreateDevice(device);
            }
        }       
    }
}
