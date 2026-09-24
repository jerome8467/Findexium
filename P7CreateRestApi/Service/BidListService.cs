using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service
{
    public class BidListService : IBidListService
    {

        private readonly IBidListRepository _bidListRepository;

        public BidListService(IBidListRepository bidListRepository)
        {
            _bidListRepository = bidListRepository;
        }

        public async Task<IEnumerable<BidList>> GetAllBidList()
        {
            return await _bidListRepository.GetAllBidList();
        }

        public async Task<BidList?> GetBidListById(int id)
        {
            return await _bidListRepository.GetBidLIstById(id);
        }

        public async Task<BidListModel?> GetBidListModelById(int id)
        {
            BidList? bidList = await _bidListRepository.GetBidLIstById(id);
            if(bidList == null)
                return null;
            return MappingBidListForModel(bidList);
        }

        public async Task<ServiceResult<BidList>> AddBidList(BidListModel bidlistModel, string userName)
        {
            var result = new ServiceResult<BidList>();
            ValidationContext context = new ValidationContext(bidlistModel);
            if (!Validator.TryValidateObject(bidlistModel, context, result.Errors, true))
                return result;

            BidList bidList = MappingBidListModelForDatabase(bidlistModel);

            bidList.CreationName = userName;
            bidList.CreationDate = DateTime.Now;
            bidList.RevisionName = userName;
            bidList.RevisionDate = DateTime.Now;

            await _bidListRepository.AddBidList(bidList);

            result.Data = bidList;
            return result;
        }

        public async Task<ServiceResult<BidList>> UpdateBidList(BidListModel bidlistModel, string userName, int id)
        {
            var result = new ServiceResult<BidList>();
            ValidationContext context = new ValidationContext(bidlistModel);

            if (!Validator.TryValidateObject(bidlistModel, context, result.Errors, true))
                return result;

            BidList bidList = MappingBidListModelForDatabase(bidlistModel);
            bidList.BidListId = id;
            bidList.RevisionName = userName;
            bidList.RevisionDate = DateTime.Now;

            bool success = await _bidListRepository.UpdateBidList(bidList);
            if (!success)
            {
                result.Errors.Add(new ValidationResult(BidListModelRessources.BidListNotFound));
                return result;
            }

            result.Data = bidList;
            return result;
        }

        public async Task<bool> DeleteBidList(int id)
        {
            return await _bidListRepository.DeleteBidList(id);
        }

        private BidList MappingBidListModelForDatabase(BidListModel bidlistModel)
        {
            BidList bidList = new BidList
            {
                Account = bidlistModel.Account,
                BidType = bidlistModel.BidType,
                BidQuantity = bidlistModel.BidQuantity,
                AskQuantity = bidlistModel.AskQuantity,
                Bid = bidlistModel.Bid,
                Ask = bidlistModel.Ask,
                Benchmark = bidlistModel.Benchmark,
                BidListDate = bidlistModel.BidListDate,
                Commentary = bidlistModel.Commentary,
                BidSecurity = bidlistModel.BidSecurity,
                BidStatus = bidlistModel.BidStatus,
                Trader = bidlistModel.Trader,
                Book = bidlistModel.Book,
                DealName = bidlistModel.DealName,
                DealType = bidlistModel.DealType,
                SourceListId = bidlistModel.SourceListId,
                Side = bidlistModel.Side
            };
            
            return bidList;
        }

        private BidListModel MappingBidListForModel(BidList bidlist)
        {
            BidListModel bidListModel = new BidListModel
            {
                Account = bidlist.Account,
                BidType = bidlist.BidType,
                BidQuantity = bidlist.BidQuantity,
                AskQuantity = bidlist.AskQuantity,
                Bid = bidlist.Bid,
                Ask = bidlist.Ask,
                Benchmark = bidlist.Benchmark,
                BidListDate = bidlist.BidListDate,
                Commentary = bidlist.Commentary,
                BidSecurity = bidlist.BidSecurity,
                BidStatus = bidlist.BidStatus,
                Trader = bidlist.Trader,
                Book = bidlist.Book,
                DealName = bidlist.DealName,
                DealType = bidlist.DealType,
                SourceListId = bidlist.SourceListId,
                Side = bidlist.Side
            };

            return bidListModel;
        }


    }
}
