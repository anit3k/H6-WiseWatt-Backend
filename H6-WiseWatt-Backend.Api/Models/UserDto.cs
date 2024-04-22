namespace H6_WiseWatt_Backend.Api.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) designed to represent user information within an API context. 
    /// This class is used to transfer user-related data between client application and backend services.
    /// </summary>
    public class UserDTO
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
