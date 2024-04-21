using H6_WiseWatt_Backend.Api.Models;
using H6_WiseWatt_Backend.Api.Utils;
using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace H6_WiseWatt_Backend.Api.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly UserDTOMapper _userMapper;
        private readonly AuthenticationUtility _utility;

        public UserController(IUserManager userManager, UserDTOMapper userMapper, AuthenticationUtility utility)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _utility = utility;
        }

        [HttpPost]
        [Route("api/user/register")]
        public async Task<IActionResult> RegisterUser(UserDTO user)
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

                if (await AddNewUser(user))
                {
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

        [HttpGet]
        [Route("api/user/get")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var user = await GetUser(userGuid);
                if (user != null)
                {
                    return Ok(user);
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

        private async Task<UserDTO> GetUser(string? userGuid)
        {
            var result = await _userManager.GetUser(new UserEntity { UserGuid = userGuid });
            return _userMapper.MapToUserDto(result);
        }

        [HttpPost]
        [Route("api/user/update")]
        [Authorize]
        public async Task<IActionResult> Update(UserDTO user)
        {
            try
            {
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                var userUpdate = await UpdateUser(user, userGuid);
                if (userUpdate != null)
                {
                    return Ok(userUpdate);
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

        [HttpPost]
        [Route("api/user/delete")]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var userGuid = _utility.GetUserGuidFromToken(User);
                if (_utility.ValidateUser(userGuid))
                {
                    return BadRequest("Invalid User");
                }

                if (userGuid == "f10774ec-bc8b-40a6-9049-32634363e298")
                {
                    return BadRequest("Test user cannot be deleted!");
                }

                var userDeleted = await DeleteUser(userGuid);
                if (userDeleted == true)
                {
                    return Ok("user has been deleted");
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

        private async Task<bool> DeleteUser(string? userGuid)
        {
            return await _userManager.DeleteCurrentUser(userGuid);
        }

        private async Task<UserDTO> UpdateUser(UserDTO user, string userGuid)
        {
            var reuslt = await _userManager.UpdateCurrentUser(_userMapper.MapToUserEntity(user, userGuid));
            return _userMapper.MapToUserDto(reuslt);
        }

        private bool IsNotValid(UserDTO user)
        {
            return user == null || string.IsNullOrWhiteSpace(user.Firstname) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Lastname);
        }

        private async Task<bool> DoUserExist(UserDTO user)
        {
            var result = await _userManager.ValidateUserByEmail(user.Email);
            return result;
        }

        private async Task<bool> AddNewUser(UserDTO user)
        {
            return await _userManager.CreateNewUser(_userMapper.MapToUserEntity(user));
        }
    }
}
