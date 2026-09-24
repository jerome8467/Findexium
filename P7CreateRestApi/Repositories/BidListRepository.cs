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

        public async Task<BidList?> GetBidLIstById(int id)
        {
            return await _DbContext.BidLists.FirstOrDefaultAsync(b => b.BidListId == id);
        }
        public async Task AddBidList(BidList bidlist)
        {
            _DbContext.BidLists.Add(bidlist);
            await _DbContext.SaveChangesAsync();
        }
        public async Task<bool> UpdateBidList(BidList bidlist)
        {
            BidList? findBidList = await GetBidLIstById(bidlist.BidListId);
            if (findBidList == null)
                return false;

            _DbContext.Entry(findBidList).CurrentValues.SetValues(bidlist);
            await _DbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteBidList(int id)
        {
            BidList? findBidList = await GetBidLIstById(id);
            if (findBidList == null)
                return false;

            _DbContext.BidLists.Remove(findBidList);
            await _DbContext.SaveChangesAsync();
            return true;
        }
    }
}
