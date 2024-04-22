using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for repository operations related to electricity price data. 
    /// It specifies the methods for interacting with the data store to retrieve and update electricity pricing information.
    /// </summary>
    public interface IElectricityPriceRepo
    {
        /// <summary>
        /// Asynchronously retrieves an electricity price based on a given timestamp. 
        /// The method returns an ElectricityPriceEntity representing the price at that specific time.
        /// </summary>
        /// <param name="timeStamp">Current time</param>
        /// <returns>ElectricityPriceEntity</returns>
        public Task<ElectricityPriceEntity> GetPrice(DateTime timeStamp);

        /// <summary>
        /// Asynchronously retrieves a list of all electricity prices. 
        /// This method returns a list of ElectricityPriceEntity, providing all available pricing information.
        /// </summary>
        /// <returns>A list of ElectricityPriceEntity</returns>
        public Task<List<ElectricityPriceEntity>> GetAllPrices();

        /// <summary>
        /// Asynchronously updates the electricity prices in the repository with a given list of ElectricityPriceEntity. 
        /// This method is used to refresh or update the price data in the data store.
        /// </summary>
        /// <param name="priceUpdate">Current price list</param>
        public Task UpdatePrices(List<ElectricityPriceEntity> priceUpdate);
    }
}
