namespace H6_WiseWatt_Backend.Security.Interfaces
{
    public interface IPasswordHasher
    {
        string GenerateSalt();
        string HashPasswordWithSalt(string password, string salt);
    }
}
