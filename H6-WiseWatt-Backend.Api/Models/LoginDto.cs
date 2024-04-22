namespace H6_WiseWatt_Backend.Api.Models
{
    /// <summary>
    /// Data Transfer Object (DTO) designed to represent user login information in an API context. 
    /// This class is used to transfer user credentials for login and authentication purposes.
    /// </summary>
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
