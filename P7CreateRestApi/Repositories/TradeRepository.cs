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

        public async Task UpdateTrade(Trade trade)
        {
            _DbContext.Update(trade);
            await _DbContext.SaveChangesAsync();
        }

        public async Task DeleteTrade(Trade trade)
        {
            _DbContext.Trades.Remove(trade);
            await _DbContext.SaveChangesAsync();

        }

    }
}
