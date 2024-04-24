using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.Domain.Services;
using Moq;

namespace H6_WiseWatt_Backend.Test
{
    [TestFixture]
    public class DeviceManagerTests
    {
        private Mock<IDeviceRepo> _mockDeviceRepo;
        private DeviceManager _deviceManager;
        private DeviceFactoryService _deviceFactory;

        [SetUp]
        public void Setup()
        {
            _mockDeviceRepo = new Mock<IDeviceRepo>();
            _deviceManager = new DeviceManager(_mockDeviceRepo.Object);
            _deviceFactory = new DeviceFactoryService();
        }

        [Test]
        public async Task DeviceSchedule_IsOnShouldBeTrue()
        {
            // Arrange
            var device = _deviceFactory.CreateDevice("Dishwasher");
            var serialNo = device.Serial;
            device.OnTime = new TimeSpan(0, 0, 0);
            device.OffTime = new TimeSpan(23, 59, 59);
            _mockDeviceRepo.Setup(r => r.GetDevice(serialNo)).ReturnsAsync(device);

            // Act
            var result = await _deviceManager.GetDevice(serialNo);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Serial, Is.EqualTo(serialNo));
            Assert.That(result.IsOn, Is.True);
        }

        [Test]
        public async Task DeviceSchedule_IsOnShouldBeFalse()
        {
            // Arrange
            var device = _deviceFactory.CreateDevice("Dryer");
            var serialNo = device.Serial;
            device.OnTime = new TimeSpan(0, 0, 0);
            device.OffTime = new TimeSpan(0, 0, 0);
            _mockDeviceRepo.Setup(r => r.GetDevice(serialNo)).ReturnsAsync(device);

            // Act
            var reuslt = await _deviceManager.GetDevice(serialNo);

            // Assert
            Assert.IsNotNull(reuslt);
            Assert.That(reuslt.Serial, Is.EqualTo(serialNo));
            Assert.That(reuslt.IsOn, Is.False);
        }

        [Test]
        public async Task DeviceManuallyOperated_IsOnShouldBetrue()
        {
            // Arrange
            var device = _deviceFactory.CreateDevice("CarCharger");
            var serialNo = device.Serial;
            device.OnTime = new TimeSpan(0, 0, 0);
            device.OffTime = new TimeSpan(0, 0, 0);
            device.IsManuallyOperated = true;
            _mockDeviceRepo.Setup(r => r.GetDevice(serialNo)).ReturnsAsync(device);

            // Act
            var reuslt = await _deviceManager.GetDevice(serialNo);

            // Assert
            Assert.IsNotNull(reuslt);
            Assert.That(reuslt.IsManuallyOperated, Is.True);
            Assert.That(reuslt.IsOn, Is.True);
        }

        [Test]
        public async Task DeviceManuallyOperated_IsOnShouldBeFalse()
        {
            // Arrange
            var device = _deviceFactory.CreateDevice("HeatPump");
            var serialNo = device.Serial;
            device.OnTime = new TimeSpan(0, 0, 0);
            device.OffTime = new TimeSpan(0, 0, 0);
            device.IsManuallyOperated = false;
            _mockDeviceRepo.Setup(r => r.GetDevice(serialNo)).ReturnsAsync(device);

            // Act
            var result = await _deviceManager.GetDevice(serialNo);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.IsManuallyOperated, Is.False);
            Assert.That(result.IsOn, Is.False);
        }

        [Test]
        public async Task DeviceUpdateTest()
        {
            // Arrange
            var device = _deviceFactory.CreateDevice("WashingMachine");
            var serialNo = device.Serial;
            device.OnTime = new TimeSpan(02, 0, 0);
            device.OffTime = new TimeSpan(05, 0, 0);
            device.IsManuallyOperated = false;
            _mockDeviceRepo.Setup(r => r.GetDevice(serialNo)).ReturnsAsync(device);

            // Act
            var result = await _deviceManager.GetDevice(serialNo);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.OnTime, Is.EqualTo(new TimeSpan(02, 0, 0)));
            Assert.That(result.OffTime, Is.EqualTo(new TimeSpan(05, 0, 0)));

            // Re-Arrange
            device.OnTime = new TimeSpan(10, 0, 0);
            device.OffTime = new TimeSpan(12, 0, 0);

            // Re-Act
            await _deviceManager.UpdateDevice(device);
            var result2 = await _deviceManager.GetDevice(serialNo);

            // Re-Assert
            Assert.IsNotNull(result);
            Assert.That(result2.OnTime, Is.EqualTo(new TimeSpan(10, 0, 0)));
            Assert.That(result2.OffTime, Is.EqualTo(new TimeSpan(12, 0, 0)));
        }

        [Test]
        public async Task GetDevicesTest()
        {
            // Arrange
            var userGuid = Guid.NewGuid().ToString();
            var devices = _deviceFactory.CreateDefaultDevices();

            foreach (var device in devices)
            {
                device.UserGuid = userGuid;
                device.EnergyConsumption = 2.0;
                device.OnTime = new TimeSpan(02, 0, 0);
                device.OffTime = new TimeSpan(05, 0, 0);
            }
            _mockDeviceRepo.Setup(r => r.GetDevices(userGuid)).ReturnsAsync(devices);

            // Act
            var result = await _deviceManager.GetDevices(userGuid);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(5));
            foreach (var device in result)
            {
                Assert.That(device.UserGuid, Is.EqualTo(userGuid));
            }
        }
    }
}
