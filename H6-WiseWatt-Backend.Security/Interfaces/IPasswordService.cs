namespace H6_WiseWatt_Backend.Security.Interfaces
{
    public interface IPasswordService
    {
        string GenerateSalt();
        string HashPasswordWithSalt(string password, string salt);
    }
}
