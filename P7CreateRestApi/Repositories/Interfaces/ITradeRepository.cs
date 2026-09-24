using P7CreateRestApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface ITradeRepository
    {
        public Task<IEnumerable<Trade>> GetAllTrade();
        public Task<Trade?> GetTradeById(int id);
        public Task AddTrade(Trade trade);
        public Task<bool> UpdateTrade(Trade trade);
        public Task<bool> DeleteTrade(int id);
    }
}
