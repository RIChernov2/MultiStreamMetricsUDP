namespace ConfigurationUDP.Instances
{
    public record DataPackage
    {
        public DataPackage(int orderNumber, double value)
        {
            OrderNumber = orderNumber;
            Value = value;
        }
        public int OrderNumber { get; init; }
        public double Value { get; init; }
    }
}
