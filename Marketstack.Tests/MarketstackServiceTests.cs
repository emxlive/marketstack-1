using Marketstack.Interfaces;
using Marketstack.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Marketstack.Tests
{
    public class MarketstackServiceTests
    {
        private readonly IMarketstackService _marketstackService;
        public MarketstackServiceTests()
        {
            var options = Options.Create(new MarketstackOptions() { ApiToken = "a49b6cfd943daff80bfd2d5103d787fb"});
            _marketstackService = new MarketstackService(options, NullLogger<MarketstackService>.Instance);
        }

        [Fact]
        public async Task GetExchanges_ReturnsExchanges()
        {
            var exchanges = await _marketstackService.GetExchanges().ToListAsync();
            Assert.NotEmpty(exchanges);
        }

        [Fact]
        public async Task GetExchangeStocks_ReturnsStocks()
        {
            var nasdaqMic = "XNAS";
            var stocks = await _marketstackService.GetExchangeStocks(nasdaqMic)
                .Take(400)
                .ToListAsync();
            Assert.True(stocks.Count == 400);
        }

        [Fact]
        public async Task GetStockEodBars_ReturnsBars()
        {
            var appleSymbol = "AAPL";
            var fromDate = DateTime.Now.AddDays(-200);
            var toDate = DateTime.Now;
            var bars = await _marketstackService.GetStockEodBars(appleSymbol, fromDate, toDate)
                .ToListAsync();            
            Assert.NotEmpty(bars);
            Assert.True(bars.Count <= 200);
        }
    }
}