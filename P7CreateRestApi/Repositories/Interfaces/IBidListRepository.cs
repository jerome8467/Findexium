using P7CreateRestApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IBidListRepository
    {

        public Task<IEnumerable<BidList>> GetAllBidList();
        public Task<BidList?> GetBidListById(int id);
        public Task AddBidList(BidList bidlist);
        public Task UpdateBidList(BidList bidlist);
        public Task DeleteBidList(BidList bidlist);

    }
}
