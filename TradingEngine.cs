using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto_Trader_Platform.Models;

namespace Auto_Trader_Platform.Services
{
    public class TradingEngine
    {
        private readonly TwelveDataService _dataService;
        private readonly List<Trade> _trades;
        private readonly Dictionary<string, decimal> _lastPrices;

        public event EventHandler<List<Trade>> OnTradesUpdated;

        public TradingEngine(string apiKey)
        {
            _dataService = new TwelveDataService(apiKey);
            _trades = new List<Trade>();
            _lastPrices = new Dictionary<string, decimal>();
        }

        public async Task ProcessMarketData(string symbol)
        {
            var marketData = await _dataService.GetRealTimePrice(symbol);
            _lastPrices[symbol] = marketData.Price;

            // Check for entry signals
            if (ShouldEnterTrade(symbol, marketData.Price))
            {
                EnterTrade(symbol, marketData.Price);
            }

            // Check open trades for exit signals
            var openTrades = _trades.Where(t => t.Symbol == symbol && t.Status == "Open").ToList();
            foreach (var trade in openTrades)
            {
                if (ShouldExitTrade(trade, marketData.Price))
                {
                    ExitTrade(trade, marketData.Price);
                }
            }

            OnTradesUpdated?.Invoke(this, _trades.ToList());
        }

        private bool ShouldEnterTrade(string symbol, decimal price)
        {
            // Implement your entry logic here
            return false; // Placeholder
        }

        private bool ShouldExitTrade(Trade trade, decimal currentPrice)
        {
            var profitPercent = (currentPrice - trade.EntryPrice) / trade.EntryPrice * 100;
            return profitPercent >= 2 || profitPercent <= -1;
        }

        private void EnterTrade(string symbol, decimal price)
        {
            var trade = new Trade
            {
                Symbol = symbol,
                EntryPrice = price,
                EntryDate = DateTime.UtcNow,
                Status = "Open",
                Quantity = 1,
                Direction = "Long"
            };
            _trades.Add(trade);
        }

        private void ExitTrade(Trade trade, decimal exitPrice)
        {
            trade.ExitPrice = exitPrice;
            trade.ExitDate = DateTime.UtcNow;
            trade.Status = "Closed";
            trade.Profit = (exitPrice - trade.EntryPrice) * trade.Quantity;
        }
    }

}
