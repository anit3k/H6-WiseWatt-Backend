using H6_WiseWatt_Backend.Domain.Entities;
using H6_WiseWatt_Backend.Domain.Interfaces;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace H6_WiseWatt_Backend.Domain.Services
{
    public class ElectricPriceService : IElectricPriceService
    {
        private readonly IElectricityPriceRepo _priceRepo;
        private readonly HttpClient _httpClient;

        public ElectricPriceService(IElectricityPriceRepo priceRepo, HttpClient httpClient)
        {
            _priceRepo = priceRepo;
            _httpClient = httpClient;
        }
        public async Task<List<ElectricityPriceEntity>> GetElectricityPricesAsync()
        {
            var allPrices = await _priceRepo.GetAllPrices();

            if ( allPrices == null || allPrices.Count == 0 )
            {
                // Determine the date range
                string startDate = DateTime.UtcNow.AddDays(-4).ToString("yyyy-MM-dd");
                string endDate = DateTime.UtcNow.AddDays(4).ToString("yyyy-MM-dd");

                // Build the URL
                string url = $"https://andelenergi.dk/?obexport_format=csv&obexport_start={startDate}&obexport_end={endDate}&obexport_region=east&obexport_tax=0&obexport_product_id=1%231%23TIMEENERGI";

                // Download CSV
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    using var stream = await response.Content.ReadAsStreamAsync();
                    using var reader = new StreamReader(stream);

                    // Skip the first line
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

                    // Update database
                    await _priceRepo.UpdatePrices(prices);

                    return prices;
                }
            }

            return allPrices;
        }

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

        private double ParseDouble(string value)
        {
            return double.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture);
        }
    }    
}
