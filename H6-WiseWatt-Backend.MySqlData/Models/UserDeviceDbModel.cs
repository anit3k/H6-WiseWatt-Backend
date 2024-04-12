namespace H6_WiseWatt_Backend.MySqlData.Models
{
    public class UserDeviceDbModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public bool IsOn { get; set; } 
        public string CustomName { get; set; }
        public string Location { get; set; } 

        public virtual UserDbModel User { get; set; }
        public virtual DeviceDbModel Device { get; set; }
    }

}
