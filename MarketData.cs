namespace Auto_Trader_Platform.Models
{
    public class MarketData
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
