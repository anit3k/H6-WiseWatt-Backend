namespace H6_WiseWatt_Backend.Domain.Entities.IotEntities
{
    public abstract class IoTDeviceBaseEntity
    {
        public string UserGuid { get; set; }
        public abstract IoTUnit DeviceType { get; }
        public string DeviceName { get; set; }
        public string Serial { get; set; }
        public bool IsOn { get; set; }
        public double EnergyConsumption { get; set; }
        public TimeSpan OnTime { get; set; }
        public TimeSpan OffTime { get; set; }

        public virtual void SetSchedule(TimeSpan onTime, TimeSpan offTime)
        {
            OnTime = onTime;
            OffTime = offTime;
        }
        public virtual void TogglePower()
        {
            IsOn = !IsOn;
        }
    }
}
