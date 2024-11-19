using System;

namespace Auto_Trader_Platform.Models
{
    public class Trade
    {
        public string Symbol { get; set; }
        public decimal EntryPrice { get; set; }
        public decimal ExitPrice { get; set; }
        public decimal Profit { get; set; }
        public decimal Quantity { get; set; }
        public string Status { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string Direction { get; set; } // "Long" or "Short"
    }
}
