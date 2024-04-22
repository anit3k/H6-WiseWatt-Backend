using H6_WiseWatt_Backend.Domain.Entities;

namespace H6_WiseWatt_Backend.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for operations related to electricity pricing services within the domain layer. 
    /// It specifies the method for retrieving electricity price data asynchronously, 
    /// supporting scenarios where electricity prices need to be fetched and updated from a backend service.
    /// </summary>
    public interface IElectricPriceService
    {
        /// <summary>
        /// Asynchronously retrieves a list of electricity prices. 
        /// This method fetches electricity pricing data from a repository or external source and returns it as a list of ElectricityPriceEntity.
        /// </summary>
        /// <returns>List of ElectricityPriceEntity</returns>
        public Task<List<ElectricityPriceEntity>> GetElectricityPricesAsync();
    }
}
