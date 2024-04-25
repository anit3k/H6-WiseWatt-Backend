using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.Domain.Services;
using Moq;

namespace H6_WiseWatt_Backend.Test
{
    [TestFixture]
    public class UserManagerTests
    {
        private Mock<IUserRepo> _mockUserRepo;
        private Mock<IDeviceRepo> _mockDeviceRepo;
        private Mock<IDeviceFactory> _mockDeviceFactory;
        private UserManager _userManager;
        private DeviceFactoryService _deviceFactory;

        [SetUp]
        public void Setup()
        {
            _mockUserRepo = new Mock<IUserRepo>();
            _mockDeviceRepo = new Mock<IDeviceRepo>();
            _mockDeviceFactory = new Mock<IDeviceFactory>();
            _userManager = new UserManager(_mockUserRepo.Object, _mockDeviceRepo.Object, _mockDeviceFactory.Object);
            _deviceFactory = new DeviceFactoryService();
        }

        [Test]
        public async Task CreateNewUser_ShouldReturnTrueAndCreateDefaultDevices()
        {
            // Arrange
            var userGuid = Guid.NewGuid().ToString();
            var user = new UserEntity { UserGuid = userGuid, Firstname = "John", Lastname = "Doe", Email = "john@example.com", Password = "YourPassword" };
            var devices = _deviceFactory.CreateDefaultDevices();

            foreach (var device in devices)
            {
                device.UserGuid = userGuid;
                device.EnergyConsumption = 2.0;
                device.OnTime = new TimeSpan(02, 0, 0);
                device.OffTime = new TimeSpan(05, 0, 0);
            }

            _mockUserRepo.Setup(r => r.CreateNewUser(user)).ReturnsAsync(userGuid);
            _mockDeviceFactory.Setup(f => f.CreateDefaultDevices()).Returns(devices);
            _mockDeviceRepo.Setup(r => r.CreateDevice(It.IsAny<IoTDeviceBaseEntity>())).Returns(Task.CompletedTask);

            // Act
            var result = await _userManager.CreateNewUser(user);

            // Assert
            Assert.IsTrue(result);
            _mockUserRepo.Verify(r => r.CreateNewUser(user), Times.Once);
            _mockDeviceFactory.Verify(f => f.CreateDefaultDevices(), Times.Once);
            _mockDeviceRepo.Verify(r => r.CreateDevice(It.IsAny<IoTDeviceBaseEntity>()), Times.Exactly(5));
        }

        [Test]
        public async Task DeleteCurrentUser_ShouldReturnTrue()
        {
            // Arrange
            var userGuid = Guid.NewGuid().ToString();
            _mockUserRepo.Setup(r => r.DeleteUser(userGuid)).ReturnsAsync(true);

            // Act
            var result = await _userManager.DeleteCurrentUser(userGuid);

            // Assert
            Assert.IsTrue(result);
            _mockUserRepo.Verify(r => r.DeleteUser(userGuid), Times.Once);
        }

        [Test]
        public async Task GetUser_ShouldReturnCorrectUser()
        {
            // Arrange
            var userGuid = Guid.NewGuid().ToString();
            var user = new UserEntity { UserGuid = userGuid, Firstname = "Jane", Lastname = "Doe", Email = "jane@example.com", Password = "YourPassword" };
            _mockUserRepo.Setup(r => r.GetUser(user)).ReturnsAsync(user);

            // Act
            var result = await _userManager.GetUser(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserGuid, Is.EqualTo(userGuid));
            Assert.That(result.Firstname, Is.EqualTo("Jane"));
            Assert.That(result.Lastname, Is.EqualTo("Doe"));
            Assert.That(result.Email, Is.EqualTo("jane@example.com"));
        }

        [Test]
        public async Task UpdateCurrentUser_ShouldReturnUpdatedUser()
        {
            // Arrange
            var userGuid = Guid.NewGuid().ToString();
            var user = new UserEntity { UserGuid = userGuid, Firstname = "John", Lastname = "Doe", Email = "john@example.com", Password = "YourPassword" };
            var updatedUser = new UserEntity { UserGuid = userGuid, Firstname = "Johnny", Lastname = "DoeDoe", Email = "johnnyboy@example.com", Password = "YourPassword" };

            _mockUserRepo.Setup(r => r.UpdateUser(user)).ReturnsAsync(updatedUser);

            // Act
            var result = await _userManager.UpdateCurrentUser(user);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.UserGuid, Is.EqualTo(userGuid));
            Assert.That(result.Firstname, Is.EqualTo("Johnny"));
            Assert.That(result.Lastname, Is.EqualTo("DoeDoe"));
            Assert.That(result.Email, Is.EqualTo("johnnyboy@example.com"));
        }

        [Test]
        public async Task ValidateUserByEmail_ShouldReturnTrueWhenUserExists()
        {
            // Arrange
            var email = "john@example.com";
            _mockUserRepo.Setup(r => r.ValidateUserEmail(email)).ReturnsAsync(true);

            // Act
            var result = await _userManager.ValidateUserByEmail(email);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
