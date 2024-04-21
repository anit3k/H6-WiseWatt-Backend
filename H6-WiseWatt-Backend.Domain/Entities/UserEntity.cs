namespace H6_WiseWatt_Backend.Domain.Entities
{
    public class UserEntity
    {
        public string UserGuid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
    }
}
