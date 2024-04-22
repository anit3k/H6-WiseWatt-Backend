using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;

namespace H6_WiseWatt_Backend.Domain.Services
{
    /// <summary>
    /// Provides functionality for retrieving and updating electricity prices within the domain layer. 
    /// It implements the IElectricPriceService interface, defining methods for fetching electricity price data, 
    /// either from a repository or from an external source via HTTP requests.
    /// </summary>
    public class ElectricPriceService : IElectricPriceService
    {
        #region fields
        private readonly IElectricityPriceRepo _priceRepo;
        private readonly HttpClient _httpClient;
        #endregion

        #region Constructor
        public ElectricPriceService(IElectricityPriceRepo priceRepo, HttpClient httpClient)
        {
            _priceRepo = priceRepo;
            _httpClient = httpClient;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Asynchronously retrieves a list of all electricity prices from the repository. 
        /// If the repository is empty or contains outdated data, it triggers a refresh of electricity prices by calling FetchNewPrices().
        /// </summary>
        /// <returns>A list of electricity prices</returns>
        public async Task<List<ElectricityPriceEntity>> GetElectricityPricesAsync()
        {
            var allPrices = await _priceRepo.GetAllPrices();

            if (allPrices == null || allPrices.Count == 0 || allPrices.Any(p => p.TimeStamp < DateTime.UtcNow.AddDays(1)))
            {
                await FetchNewPrices();
                return await _priceRepo.GetAllPrices();
            }

            return allPrices;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Fetches new electricity prices from an external source, parses the data, and updates the repository. 
        /// It uses HTTP to retrieve data in CSV format and processes each line to extract electricity price information.
        /// </summary>
        private async Task FetchNewPrices()
        {
            string startDate = DateTime.UtcNow.AddDays(-4).ToString("yyyy-MM-dd");
            string endDate = DateTime.UtcNow.AddDays(4).ToString("yyyy-MM-dd");

            string url = $"https://andelenergi.dk/?obexport_format=csv&obexport_start={startDate}&obexport_end={endDate}&obexport_region=east&obexport_tax=0&obexport_product_id=1%231%23TIMEENERGI";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);
                reader.ReadLine();

                var prices = new List<ElectricityPriceEntity>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var entity = ParseLine(line);
                    if (entity != null)
                    {
                        prices.Add(entity);
                    }
                }

                await _priceRepo.UpdatePrices(prices);
            }
        }

        /// <summary>
        /// Parses a line from the CSV file, extracting electricity price data and returning an ElectricityPriceEntity. 
        /// If the line format is incorrect, it throws a FormatException.
        /// </summary>
        private ElectricityPriceEntity ParseLine(string line)
        {
            var matches = Regex.Matches(line, "\"([^\"]*)\"");
            if (matches.Count == 4)
            {
                return new ElectricityPriceEntity
                {
                    TimeStamp = DateTime.ParseExact(matches[0].Groups[1].Value, "dd.MM.yyyy - HH:mm", CultureInfo.InvariantCulture),
                    PricePerKwh = ParseDouble(matches[1].Groups[1].Value),
                    TransportAndDuties = ParseDouble(matches[2].Groups[1].Value),
                    TotalPrice = ParseDouble(matches[3].Groups[1].Value)
                };
            }
            throw new FormatException("Line format incorrect, expected 4 columns.");
        }

        /// <summary>
        /// Converts a string to a double, handling cultural differences in number formatting (e.g., using . as the decimal separator).
        /// </summary>
        private double ParseDouble(string value)
        {
            return double.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
        }
        #endregion
    }    
}
