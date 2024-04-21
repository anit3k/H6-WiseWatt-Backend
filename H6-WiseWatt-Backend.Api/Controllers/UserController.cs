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

        private async Task<UserDTO> UpdateUser(UserDTO user, string userGuid)
        {
            var reuslt = await _userManager.UpdateCurrentUser(_userMapper.MapToUserEntity(user, userGuid));
            return _userMapper.MapToUserDto(reuslt);
        }

        private bool IsNotValid(UserDTO user)
        {
            return user == null || string.IsNullOrWhiteSpace(user.Firstname) && string.IsNullOrWhiteSpace(user.Password) && string.IsNullOrWhiteSpace(user.Email);
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
