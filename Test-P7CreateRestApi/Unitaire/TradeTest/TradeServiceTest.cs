using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Service;
using P7CreateRestApi.Service.Interfaces;
using System.Collections;
using System.Diagnostics;
using System.Security.Principal;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.TradeTest
{
    public class TradeServiceTest : IDisposable
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly ITradeService _tradeService;
        private readonly LocalDbContext _dbContext;

        public TradeServiceTest()
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite("Data source=:memory:")
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _dbContext.Trades.Add(CreateValideTrade(1, "Account1", "Createur1"));
            _dbContext.Trades.Add(CreateValideTrade(2, "Account2", "Createur2"));
            _dbContext.Trades.Add(CreateValideTrade(3, "Account3", "Createur3"));
            _dbContext.Trades.Add(CreateValideTrade(4, "Account4", "Createur4"));
            _dbContext.SaveChanges();

            _tradeRepository = new TradeRepository(_dbContext);
            _tradeService = new TradeService(_tradeRepository);
        }

        public Trade CreateValideTrade(int id, string Account, string CreationName)
        {
            return new Trade
            {
                TradeId = id,
                Account = Account,
                CreationName = CreationName,
                CreationDate = DateTime.Now,
                RevisionName = CreationName,
                RevisionDate = DateTime.Now,
                AccountType = "Test",
                TradeSecurity = "Test",
                TradeStatus = "Test",
                Trader = "Test",
                Benchmark = "Test",
                Book = "Test",
                DealName = "Test",
                DealType = "Test",
                SourceListId = "Test",
                Side = "Test",
            };
        }

        public void Dispose()
        {
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task Service_GetAllTrade()
        {
            // ACT
            IEnumerable<Trade> trades = await _tradeService.GetAllTrade();

            // ASSERT
            Assert.Equal(4, trades.Count());
        }

        [Fact]
        public async Task Service_GetTradeById()
        {
            // ACT
            Trade? trade = await _tradeService.GetTradeById(1);

            // ASSERT
            Assert.NotNull(trade);
            Assert.Equal(1, trade?.TradeId);
            Assert.Equal("Account1", trade?.Account);
        }

        [Fact]
        public async Task Service_GetTradeModelById()
        {
            // ACT
            TradeModel? tradeModel = await _tradeService.GetTradeModelById(2);

            // ASSERT
            Assert.NotNull(tradeModel);
            Assert.Equal("Account2", tradeModel?.Account);
        }

        [Fact]
        public async Task Service_UpdateTrade()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = "Account-1-Update",
                AccountType = "Test",
                TradeSecurity = "Test",
                TradeStatus = "Test",
                Trader = "Test",
                Benchmark = "Test",
                Book = "Test",
                DealName = "Test",
                DealType = "Test",
                SourceListId = "Test",
                Side = "Test",
            };

            // ACT
            ServiceResult<Trade> serviceResult = await _tradeService.UpdateTrade(tradeModel, "JeromeForUpdate", 1);
            Trade? trade = await _dbContext.Trades
                .AsNoTracking()
                .FirstAsync(i => i.TradeId == 1);

            // ASSERT
            Assert.NotNull (trade);
            Assert.Equal(1,trade.TradeId);
            Assert.Equal("Account-1-Update", trade.Account);
            Assert.Equal("Createur1", trade.CreationName);
            Assert.Equal("JeromeForUpdate", trade.RevisionName);
            Assert.InRange(trade!.RevisionDate!.Value, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.Empty(serviceResult?.Errors);
        }

        [Fact]
        public async Task Service_AddTrade()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = "Account5",
                AccountType = "Test",
                TradeSecurity = "Test",
                TradeStatus = "Test",
                Trader = "Test",
                Benchmark = "Test",
                Book = "Test",
                DealName = "Test",
                DealType = "Test",
                SourceListId = "Test",
                Side = "Test",
            };

            // ACT
            ServiceResult<Trade> serviceResult = await _tradeService.AddTrade(tradeModel, "JeromeForNew");
            Trade? trade = await _dbContext.Trades
                .AsNoTracking()
                .OrderBy(i => i.TradeId).LastAsync();
            int tradeCount = await _dbContext.Trades.CountAsync();

            // ASSERT
            Assert.Empty(serviceResult?.Errors);
            Assert.NotNull(trade);
            Assert.NotNull(trade?.RevisionDate);
            Assert.NotNull(trade?.CreationDate);
            Assert.InRange(trade!.RevisionDate!.Value, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.InRange(trade!.CreationDate!.Value, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.Equal("Account5", trade?.Account);
            Assert.Equal(5, trade?.TradeId);
            Assert.Equal("JeromeForNew", trade?.CreationName);
            Assert.Equal("JeromeForNew", trade?.RevisionName);
            Assert.Equal(5, tradeCount);

        }

        [Fact]
        public async Task Service_DeleteTrade()
        {
            // ACT
            await _tradeService.DeleteTrade(2);
            IEnumerable<Trade> trades = await _dbContext.Trades
                .ToListAsync();

            // ASSERT
            Assert.Equal(3, trades.Count());
            Assert.Contains(trades, t => t.TradeId == 1);
            Assert.Contains(trades, t => t.TradeId == 3);
            Assert.Contains(trades, t => t.TradeId == 4);
            Assert.DoesNotContain(trades, t => t.TradeId == 2);
        }


    }
}
