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

        public async Task<IEnumerable<BidListDto>> GetAllBidList()
        {
            IEnumerable<BidList> bidList = await _bidListRepository.GetAllBidList();
            List<BidListDto> BidListDtos = new List<BidListDto>();
            foreach (var dto in bidList)
            {
                BidListDtos.Add(MappingBidListForDto(dto));
            }

            return BidListDtos.ToList();
        }

        public async Task<BidListDto?> GetBidListById(int id)
        {
            BidList? bidList = await _bidListRepository.GetBidListById(id);
            if(bidList == null)
                return null;
            return MappingBidListForDto(bidList);
        }

        public async Task<BidListModel?> GetBidListModelById(int id)
        {
            BidList? bidList = await _bidListRepository.GetBidListById(id);
            if(bidList == null)
                return null;
            return MappingBidListForModel(bidList);
        }

        public async Task<ServiceResult<BidListDto>> AddBidList(BidListModel bidlistModel, string userName)
        {
            var result = new ServiceResult<BidListDto>();
            ValidationContext context = new ValidationContext(bidlistModel);
            if (!Validator.TryValidateObject(bidlistModel, context, result.Errors, true))
                return result;

            BidList bidList = MappingBidListModelForDatabase(bidlistModel, new BidList());

            bidList.CreationName = userName;
            bidList.CreationDate = DateTime.Now;
            bidList.RevisionName = userName;
            bidList.RevisionDate = DateTime.Now;

            await _bidListRepository.AddBidList(bidList);

            result.Data = MappingBidListForDto(bidList);
            return result;
        }

        public async Task<ServiceResult<BidListDto>> UpdateBidList(BidListModel bidlistModel, string userName, int id)
        {
            var result = new ServiceResult<BidListDto>();
            ValidationContext context = new ValidationContext(bidlistModel);

            if (!Validator.TryValidateObject(bidlistModel, context, result.Errors, true))
                return result;

            BidList? findBidList = await _bidListRepository.GetBidListById(id);
            if (findBidList == null)
            {
                result.Errors.Add(new ValidationResult(BidListModelRessources.BidListNotFound));
                return result;
            }

            findBidList = MappingBidListModelForDatabase(bidlistModel, findBidList);
            findBidList.RevisionName = userName;
            findBidList.RevisionDate = DateTime.Now;

            await _bidListRepository.UpdateBidList(findBidList);

            result.Data = MappingBidListForDto(findBidList);
            return result;
        }

        public async Task<List<ValidationResult>> DeleteBidList(int id)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            BidList? findBidList = await _bidListRepository.GetBidListById(id);
            if (findBidList == null)
            {
                result.Add(new ValidationResult(BidListModelRessources.BidListNotFound));
                return result;
            }

            await _bidListRepository.DeleteBidList(findBidList);
            return result;
        }

        private BidList MappingBidListModelForDatabase(BidListModel bidlistModel, BidList bidList)
        {
            bidList.Account = bidlistModel.Account;
            bidList.BidType = bidlistModel.BidType;
            bidList.BidQuantity = bidlistModel.BidQuantity;
            bidList.AskQuantity = bidlistModel.AskQuantity;
            bidList.Bid = bidlistModel.Bid;
            bidList.Ask = bidlistModel.Ask;
            bidList.Benchmark = bidlistModel.Benchmark;
            bidList.BidListDate = bidlistModel.BidListDate;
            bidList.Commentary = bidlistModel.Commentary;
            bidList.BidSecurity = bidlistModel.BidSecurity;
            bidList.BidStatus = bidlistModel.BidStatus;
            bidList.Trader = bidlistModel.Trader;
            bidList.Book = bidlistModel.Book;
            bidList.DealName = bidlistModel.DealName;
            bidList.DealType = bidlistModel.DealType;
            bidList.SourceListId = bidlistModel.SourceListId;
            bidList.Side = bidlistModel.Side;
            
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

        private BidListDto MappingBidListForDto(BidList bidlist)
        {
            BidListDto bidListDto = new BidListDto
            {
                BidListId = bidlist.BidListId,
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
                CreationName = bidlist.CreationName,
                CreationDate = bidlist.CreationDate,
                RevisionName = bidlist.RevisionName,
                RevisionDate = bidlist.RevisionDate,
                DealName = bidlist.DealName,
                DealType = bidlist.DealType,
                SourceListId = bidlist.SourceListId,
                Side = bidlist.Side
            };
            return bidListDto;
        }


    }
}
