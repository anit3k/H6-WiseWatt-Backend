using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using Newtonsoft.Json;

namespace H6_WiseWatt_Backend.FileDataStorage
{
    public class FileDeviceStorageRepo : IUserDeviceStorageRepo
    {
        private const string DevicesDirectory = "FileStorage";
        
        public async Task<List<IoTDeviceBaseEntity>> GetDevices(string userGuid)
        {
            string filePath = GetFilePath(userGuid);
            if (!File.Exists(filePath))
                return new List<IoTDeviceBaseEntity>();

            var jsonData = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<List<IoTDeviceBaseEntity>>(jsonData) ?? new List<IoTDeviceBaseEntity>();
        }

        public async Task CreateDevice(IoTDeviceBaseEntity device)
        {
            string filePath = GetFilePath(device.UserGuid);
            List<IoTDeviceBaseEntity> devices = await LoadOrCreateDevices(filePath);

            devices.Add(device);

            await SaveDevicesToFile(devices, filePath);
        }

        public async Task UpdateDevice(IoTDeviceBaseEntity device)
        {
            string filePath = GetFilePath(device.UserGuid);
            List<IoTDeviceBaseEntity> devices = await LoadOrCreateDevices(filePath);

            var index = devices.FindIndex(d => d.Serial == device.Serial);
            if (index != -1)
            {
                devices[index] = device;
            }
            else
            {
                devices.Add(device);
            }

            await SaveDevicesToFile(devices, filePath);
        }

        public async Task DeleteDevice(IoTDeviceBaseEntity device)
        {
            string filePath = GetFilePath(device.UserGuid);
            List<IoTDeviceBaseEntity> devices = await LoadOrCreateDevices(filePath);

            devices.RemoveAll(d => d.Serial == device.Serial);

            await SaveDevicesToFile(devices, filePath);
        }
        private string GetFilePath(string userGuid)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullDirectoryPath = Path.Combine(basePath, DevicesDirectory);

            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            return Path.Combine(fullDirectoryPath, $"{userGuid}_devices.json");
        }

        private async Task<List<IoTDeviceBaseEntity>> LoadOrCreateDevices(string filePath)
        {
            if (File.Exists(filePath))
            {
                var jsonData = await File.ReadAllTextAsync(filePath);
                return JsonConvert.DeserializeObject<List<IoTDeviceBaseEntity>>(jsonData) ?? new List<IoTDeviceBaseEntity>();
            }

            return new List<IoTDeviceBaseEntity>();
        }        

        private async Task SaveDevicesToFile(List<IoTDeviceBaseEntity> devices, string filePath)
        {
            var jsonData = JsonConvert.SerializeObject(devices, Formatting.Indented);
            await File.WriteAllTextAsync(filePath, jsonData);
        }
    }
}
