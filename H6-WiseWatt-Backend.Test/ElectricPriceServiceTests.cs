using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using H6_WiseWatt_Backend.Domain.Services;
using Moq;

namespace H6_WiseWatt_Backend.Tests
{
    [TestFixture]
    public class ElectricPriceServiceTests
    {
        private Mock<IElectricityPriceRepo> _mockPriceRepo;
        private Mock<HttpClient> _mockHttpClient;
        private ElectricPriceService _electricPriceService;

        [SetUp]
        public void Setup()
        {
            _mockPriceRepo = new Mock<IElectricityPriceRepo>();
            _mockHttpClient = new Mock<HttpClient>();
            _electricPriceService = new ElectricPriceService(_mockPriceRepo.Object, _mockHttpClient.Object);
        }

        [Test]
        public async Task GetElectricityPricesAsync_ShouldFetchPricesIfNoneAvailable()
        {
            // Arrange
            _mockPriceRepo.Setup(r => r.GetAllPrices()).ReturnsAsync(new List<ElectricityPriceEntity>());
            _mockPriceRepo.Setup(r => r.UpdatePrices(It.IsAny<List<ElectricityPriceEntity>>())).Returns(Task.CompletedTask);

            // Act
            var prices = await _electricPriceService.GetElectricityPricesAsync();

            // Assert
            Assert.IsNotNull(prices);
            _mockPriceRepo.Verify(r => r.UpdatePrices(It.IsAny<List<ElectricityPriceEntity>>()), Times.Once);
        }       
    }
}
