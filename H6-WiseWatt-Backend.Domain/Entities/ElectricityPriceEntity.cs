namespace H6_WiseWatt_Backend.Domain.Entities
{
    /// <summary>
    ///  Data structure for electricity pricing within the domain layer. 
    ///  This class encapsulates key information about electricity prices, 
    ///  allowing backend services to manage and interact with electricity price data effectively.
    /// </summary>
    public class ElectricityPriceEntity
    {
        #region Properties
        public DateTime TimeStamp { get; set; }
        public double PricePerKwh { get; set; }
        public double TransportAndDuties { get; set; }
        public double TotalPrice { get; set; }
        #endregion
    }
}
