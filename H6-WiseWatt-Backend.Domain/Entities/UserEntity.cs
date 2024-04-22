namespace H6_WiseWatt_Backend.Domain.Entities
{
    /// <summary>
    /// This class encapsulates key attributes that describe a user, 
    /// providing a structure to manage user-related information within domain layer.
    /// </summary>
    public class UserEntity
    {
        #region Properties
        public string UserGuid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
        #endregion
    }
}
