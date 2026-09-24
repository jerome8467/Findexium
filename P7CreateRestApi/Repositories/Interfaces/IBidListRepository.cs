using P7CreateRestApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IBidListRepository
    {

        public Task<IEnumerable<BidList>> GetAllBidList();
        public Task<BidList?> GetBidLIstById(int id);
        public Task AddBidList(BidList bidlist);
        public Task<bool> UpdateBidList(BidList bidlist);
        public Task<bool> DeleteBidList(int id);

    }
}
