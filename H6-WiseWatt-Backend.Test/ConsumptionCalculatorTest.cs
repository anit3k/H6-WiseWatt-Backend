using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.Domain.Services;
using Moq;

namespace H6_WiseWatt_Backend.Tests
{
    [TestFixture]
    public class ConsumptionCalculatorTests
    {
        private Mock<IDeviceManager> _mockDeviceManager;
        private Mock<IElectricPriceService> _mockElectricPriceService;
        private ConsumptionCalculator _calculator;
        private DeviceFactoryService _deviceFactory;

        [SetUp]
        public void Setup()
        {
            _mockDeviceManager = new Mock<IDeviceManager>();
            _mockElectricPriceService = new Mock<IElectricPriceService>();
            _calculator = new ConsumptionCalculator(_mockDeviceManager.Object, _mockElectricPriceService.Object);
            _deviceFactory = new DeviceFactoryService();
        }

        [Test]
        public async Task GetSummaryOfDailyConsumption_ReturnsCorrectSummary()
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

            var prices = CreateHourlyElectricityPrices();

            _mockDeviceManager.Setup(m => m.GetDevices(It.IsAny<string>())).ReturnsAsync(devices);
            _mockElectricPriceService.Setup(m => m.GetElectricityPricesAsync()).ReturnsAsync(prices);

            // Act
            var result = await _calculator.GetSummaryOfDailyConsumption(userGuid);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(6));
            Assert.That(6, Is.EqualTo(result[0].Item2)); // current consumption
            Assert.That(result[0].Item3, Is.EqualTo(1.2000000000000002)); // current cost
            Assert.That(6, Is.EqualTo(result[1].Item2)); 
            Assert.That(result[1].Item3, Is.EqualTo(1.2000000000000002));
            Assert.That(6, Is.EqualTo(result[2].Item2));   
            Assert.That(result[2].Item3, Is.EqualTo(1.2000000000000002));
            Assert.That(6, Is.EqualTo(result[3].Item2)); 
            Assert.That(result[3].Item3, Is.EqualTo(1.2000000000000002));
            Assert.That(6, Is.EqualTo(result[4].Item2)); 
            Assert.That(result[4].Item3, Is.EqualTo(1.2000000000000002));
            Assert.That(30, Is.EqualTo(result[5].Item2)); // total consumption
            Assert.That(result[5].Item3, Is.EqualTo(6.000000000000001)); // total cost
        }

        [Test]
        public async Task GetDailyPercentageByDevice_ReturnsCorrectPercentages()
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

            _mockDeviceManager.Setup(m => m.GetDevices(It.IsAny<string>())).ReturnsAsync(devices);

            // Act
            var result = await _calculator.GetDailyPercentageByDevice(userGuid);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.Greater(result["Opvaskemaskine"], 0);
            Assert.Greater(result["Tørretumbler"], 0);
            Assert.Greater(result["Ladestander"], 0);
            Assert.Greater(result["Varmepumpe"], 0);
            Assert.Greater(result["Vaskemaskine"], 0);
            foreach (var device in result)
            {
                Assert.That(device.Value, Is.EqualTo(20));
            }
        }

        [Test]
        public async Task GetHourlyConsumptionByDevice_ReturnsCorrectHourlyData()
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

            _mockDeviceManager.Setup(m => m.GetDevices(It.IsAny<string>())).ReturnsAsync(devices);

            // Act
            var result = await _calculator.GetHourlyConsumptionByDevice(userGuid);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.IsTrue(result.ContainsKey("Opvaskemaskine")); 
            Assert.That(result["Opvaskemaskine"].Count, Is.EqualTo(24));
            Assert.IsTrue(result.ContainsKey("Tørretumbler")); 
            Assert.That(result["Tørretumbler"].Count, Is.EqualTo(24));
            Assert.IsTrue(result.ContainsKey("Ladestander")); 
            Assert.That(result["Ladestander"].Count, Is.EqualTo(24));
            Assert.IsTrue(result.ContainsKey("Varmepumpe")); 
            Assert.That(result["Varmepumpe"].Count, Is.EqualTo(24));
            Assert.IsTrue(result.ContainsKey("Vaskemaskine")); 
            Assert.That(result["Vaskemaskine"].Count, Is.EqualTo(24));

            foreach (var device in result)
            {
                for (int i = 0; i < device.Value.Count; i++)
                {
                    if (i == 2 || i == 3 || i == 4)

                    {
                        Assert.That(device.Value[i], Is.EqualTo(2.0));
                    }
                }
            }
        }

        private List<ElectricityPriceEntity> CreateHourlyElectricityPrices()
        {
            var prices = new List<ElectricityPriceEntity>();
            var baseDate = new DateTime(2024, 1, 1, 0, 0, 0);

            for (int hour = 0; hour < 24; hour++)
            {
                var timeStamp = baseDate.AddHours(hour);
                var totalPrice = 0.2;

                prices.Add(new ElectricityPriceEntity
                {
                    TimeStamp = timeStamp,
                    TotalPrice = totalPrice
                });
            }

            return prices;
        }
    }
}
