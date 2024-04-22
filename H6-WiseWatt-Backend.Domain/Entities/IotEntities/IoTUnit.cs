namespace H6_WiseWatt_Backend.Domain.Entities.IotEntities
{
    /// <summary>
    /// Used to represent a fixed set of related values, 
    /// allowing code to refer to these values by name instead of numeric indices, improving readability and maintainability.
    /// </summary>
    public enum IoTUnit
    {
        Dishwasher = 0,
        Dryer = 1,
        CarCharger = 2,
        HeatPump = 3,
        WashingMachine = 4,
    }
}
