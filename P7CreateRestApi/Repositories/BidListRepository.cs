using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories.Interfaces;
namespace P7CreateRestApi.Repositories
{
    public class BidListRepository:IBidListRepository
    {
        private readonly LocalDbContext _DbContext;

        public BidListRepository(LocalDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<IEnumerable<BidList>> GetAllBidList()
        {
            return await _DbContext.BidLists.ToListAsync();
        }

        public async Task<BidList?> GetBidListById(int id)
        {
            return await _DbContext.BidLists.FirstOrDefaultAsync(b => b.BidListId == id);
        }
        public async Task AddBidList(BidList bidlist)
        {
            _DbContext.BidLists.Add(bidlist);
            await _DbContext.SaveChangesAsync();
        }
        public async Task UpdateBidList(BidList bidlist)
        {
            _DbContext.Update(bidlist);
            await _DbContext.SaveChangesAsync();
        }
        public async Task DeleteBidList(BidList bidlist)
        {
            _DbContext.BidLists.Remove(bidlist);
            await _DbContext.SaveChangesAsync();
        }
    }
}
