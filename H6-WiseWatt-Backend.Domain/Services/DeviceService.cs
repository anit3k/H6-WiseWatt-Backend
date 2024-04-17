﻿using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;

namespace H6_WiseWatt_Backend.Domain.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepo _deviceRepo;

        public DeviceService(IDeviceRepo deviceRepo)
        {
            _deviceRepo = deviceRepo;
        }
        public async Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid)
        {
            var devices = await _deviceRepo.GetDevices(userGuid);
            foreach (var device in devices)
            {
                UpdateOnStatus(device, DateTime.Now.TimeOfDay);
            }
            return devices;
        }

        public async Task UpdateDevice(IoTDeviceBaseEntity device)
        {
            UpdateOnStatus(device, DateTime.Now.TimeOfDay);
            await _deviceRepo.UpdateDevice(device);            
        }

        private void UpdateOnStatus(IoTDeviceBaseEntity device, TimeSpan currentTime)
        {
            if (device.IsManuallyOperated)
            {
                device.IsOn = true;
            }
            else
            {
                if (device.OnTime <= device.OffTime)
                {
                    device.IsOn = (currentTime >= device.OnTime && currentTime <= device.OffTime);
                }
                else
                {
                    device.IsOn = (currentTime >= device.OnTime || currentTime <= device.OffTime); // Over midnight case
                }
            }
        }        
    }
}