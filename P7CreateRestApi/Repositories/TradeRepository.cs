using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories.Interfaces;
using System.Collections;
using System.Diagnostics;

namespace P7CreateRestApi.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly LocalDbContext _DbContext;

        public TradeRepository(LocalDbContext localDbContext)
        {
            _DbContext = localDbContext;
        }


        public async Task<IEnumerable<Trade>> GetAllTrade()
        {
            return await _DbContext.Trades.ToListAsync();
        }

        public async Task<Trade?> GetTradeById(int id)
        {
            return await _DbContext.Trades.FirstOrDefaultAsync(t => t.TradeId == id);
        }

        public async Task AddTrade(Trade trade)
        {
            _DbContext.Trades.Add(trade);
            await _DbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateTrade(Trade trade)
        {
            Trade? findTrade = await GetTradeById(trade.TradeId);
            if (findTrade == null)
                return false;

            _DbContext.Entry(findTrade).CurrentValues.SetValues(trade);
            await _DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTrade(int id)
        {
            Trade? findTrade = await GetTradeById(id);
            if (findTrade == null)
                return false;

            _DbContext.Trades.Update(findTrade);
            await _DbContext.SaveChangesAsync();
            return true;
        }

    }
}
