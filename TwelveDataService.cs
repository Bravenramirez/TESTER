using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Auto_Trader_Platform.Models;

namespace Auto_Trader_Platform.Services
{
    public class TwelveDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.twelvedata.com";

        public TwelveDataService(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<MarketData> GetRealTimePrice(string symbol)
        {
            var endpoint = $"/price?symbol={symbol}&apikey={_apiKey}";
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TwelveDataPrice>(content);

            return new MarketData
            {
                Symbol = symbol,
                Price = decimal.Parse(data.Price),
                Timestamp = DateTime.UtcNow
            };
        }
    }

}
