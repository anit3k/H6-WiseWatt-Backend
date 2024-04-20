using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Security.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateJSonWebToken(UserEntity user);
    }
}
