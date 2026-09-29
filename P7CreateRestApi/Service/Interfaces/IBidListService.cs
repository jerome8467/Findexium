using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service.Interfaces
{
    public interface IBidListService
    {
        public Task<IEnumerable<BidList>> GetAllBidList();
        public Task<BidList?> GetBidListById(int id);
        public Task<BidListModel?> GetBidListModelById(int id);
        public Task<ServiceResult<BidList>> AddBidList(BidListModel bidlistModel, string userName);
        public Task<ServiceResult<BidList>> UpdateBidList(BidListModel bidlistModel, string userName, int id);
        public Task<List<ValidationResult>> DeleteBidList(int id);
    }
}
