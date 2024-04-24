using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
using H6_WiseWatt_Backend.Domain.Services;

namespace H6_WiseWatt_Backend.Test
{
    [TestFixture]
    public class DeviceFactoryServiceTests
    {
        private DeviceFactoryService _deviceFactory;

        [SetUp]
        public void Setup()
        {
            _deviceFactory = new DeviceFactoryService();
        }

        [Test]
        public void CreateDevice_ReturnsCorrectDeviceType()
        {
            // Arrange & Act
            var dishwasher = _deviceFactory.CreateDevice("Dishwasher");
            var dryer = _deviceFactory.CreateDevice("Dryer");
            var carCharger = _deviceFactory.CreateDevice("CarCharger");
            var heatPump = _deviceFactory.CreateDevice("HeatPump");
            var washingMachine = _deviceFactory.CreateDevice("WashingMachine");

            // Assert
            Assert.IsInstanceOf<DishwasherEntity>(dishwasher);
            Assert.IsInstanceOf<DryerEntity>(dryer);
            Assert.IsInstanceOf<ElectricCarChargerEntity>(carCharger);
            Assert.IsInstanceOf<HeatPumpEntity>(heatPump);
            Assert.IsInstanceOf<WashingMachineEntity>(washingMachine);
        }

        [Test]
        public void CreateDevice_ThrowsArgumentExceptionOnUnknownType()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => _deviceFactory.CreateDevice("UnknownType"));
        }

        [Test]
        public void CreateDefaultDevices_ReturnsExpectedNumberOfDevices()
        {
            // Act
            var devices = _deviceFactory.CreateDefaultDevices();

            // Assert
            Assert.IsNotNull(devices);
            Assert.That(devices.Count, Is.EqualTo(5));
        }

        [Test]
        public void GetRandomConsumption_ReturnsRandomValues()
        {
            // Arrange & Act
            var dishwasher = _deviceFactory.CreateDevice("Dishwasher");
            var dishwasher2 = _deviceFactory.CreateDevice("Dishwasher");
            double min = 0.8;
            double max = 1.5;

            // Assert
            Assert.That(dishwasher.EnergyConsumption != dishwasher2.EnergyConsumption);
            Assert.That(dishwasher.EnergyConsumption, Is.GreaterThanOrEqualTo(min));
            Assert.That(dishwasher.EnergyConsumption, Is.LessThanOrEqualTo(max));
        }

        [Test]
        public void GetRandomSerial_ReturnsUniqueString()
        {
            // Act
            var dryer = _deviceFactory.CreateDevice("Dryer");
            var dryer2 = _deviceFactory.CreateDevice("Dryer");

            // Assert
            Assert.IsNotNull(dryer);
            Assert.IsNotNull(dryer2);
            Assert.That(dryer.Serial, Is.Not.EqualTo(dryer2.Serial));
        }
    }
}
