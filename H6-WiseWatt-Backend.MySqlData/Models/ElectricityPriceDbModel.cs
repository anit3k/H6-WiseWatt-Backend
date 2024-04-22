namespace H6_WiseWatt_Backend.MySqlData.Models
{
    /// <summary>
    /// Data model for storing electricity price-related information in a MySQL database. 
    /// It corresponds to the ElectricityPriceEntity class used in the application's domain logic.
    /// </summary>
    public class ElectricityPriceDbModel
    {
        #region Properties
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double PricePerKwh { get; set; }
        public double TransportAndDuties { get; set; }
        public double TotalPrice { get; set; }
        #endregion
    }
}
