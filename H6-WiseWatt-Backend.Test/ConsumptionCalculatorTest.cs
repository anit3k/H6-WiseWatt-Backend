using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Entities.IotEntities;
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
            var summary = await _calculator.GetSummaryOfDailyConsumption(userGuid);

            // Assert
            Assert.IsNotNull(summary);
            Assert.That(summary.Count, Is.EqualTo(6));
            Assert.That(6, Is.EqualTo(summary[0].Item2)); // current consumption
            Assert.That(summary[0].Item3, Is.EqualTo(1.2000000000000002)); // current cost
            Assert.That(6, Is.EqualTo(summary[1].Item2)); 
            Assert.That(summary[1].Item3, Is.EqualTo(1.2000000000000002));
            Assert.That(6, Is.EqualTo(summary[2].Item2));   
            Assert.That(summary[2].Item3, Is.EqualTo(1.2000000000000002));
            Assert.That(6, Is.EqualTo(summary[3].Item2)); 
            Assert.That(summary[3].Item3, Is.EqualTo(1.2000000000000002));
            Assert.That(6, Is.EqualTo(summary[4].Item2)); 
            Assert.That(summary[4].Item3, Is.EqualTo(1.2000000000000002));
            Assert.That(30, Is.EqualTo(summary[5].Item2)); // total consumption
            Assert.That(summary[5].Item3, Is.EqualTo(6.000000000000001)); // total cost
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
            var percentages = await _calculator.GetDailyPercentageByDevice(userGuid);

            // Assert
            Assert.IsNotNull(percentages);
            Assert.That(percentages.Count, Is.EqualTo(5));
            Assert.Greater(percentages["Opvaskemaskine"], 0);
            Assert.Greater(percentages["Tørretumbler"], 0);
            Assert.Greater(percentages["Ladestander"], 0);
            Assert.Greater(percentages["Varmepumpe"], 0);
            Assert.Greater(percentages["Vaskemaskine"], 0);
            foreach (var device in percentages)
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
            var hourlyData = await _calculator.GetHourlyConsumptionByDevice(userGuid);

            // Assert
            Assert.IsNotNull(hourlyData);
            Assert.That(hourlyData.Count, Is.EqualTo(5));
            Assert.IsTrue(hourlyData.ContainsKey("Opvaskemaskine")); 
            Assert.That(hourlyData["Opvaskemaskine"].Count, Is.EqualTo(24));
            Assert.IsTrue(hourlyData.ContainsKey("Tørretumbler")); 
            Assert.That(hourlyData["Tørretumbler"].Count, Is.EqualTo(24));
            Assert.IsTrue(hourlyData.ContainsKey("Ladestander")); 
            Assert.That(hourlyData["Ladestander"].Count, Is.EqualTo(24));
            Assert.IsTrue(hourlyData.ContainsKey("Varmepumpe")); 
            Assert.That(hourlyData["Varmepumpe"].Count, Is.EqualTo(24));
            Assert.IsTrue(hourlyData.ContainsKey("Vaskemaskine")); 
            Assert.That(hourlyData["Vaskemaskine"].Count, Is.EqualTo(24));

            foreach (var device in hourlyData)
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
