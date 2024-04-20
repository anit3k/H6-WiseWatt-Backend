namespace H6_WiseWatt_Backend.MySqlData.Models
{
    public class UserDbModel
    {
        public int Id { get; set; }
        public string UserGuid { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
