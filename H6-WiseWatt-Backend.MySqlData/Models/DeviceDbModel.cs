namespace H6_WiseWatt_Backend.MySqlData.Models
{
    public class DeviceDbModel
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public double PowerConsumptionPerHour { get; set; }
        public virtual ICollection<UserDeviceDbModel> UserDevices { get; set; } = new List<UserDeviceDbModel>();
    }
}
