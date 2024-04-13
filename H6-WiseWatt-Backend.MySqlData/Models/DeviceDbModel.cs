namespace H6_WiseWatt_Backend.MySqlData.Models
{
    public class DeviceDbModel
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public double PowerConsumptionPerHour { get; set; }
        public bool IsOn { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public virtual ICollection<UserDeviceDbModel> UserDevices { get; set; } = new List<UserDeviceDbModel>();
    }
}
