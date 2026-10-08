using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service
{
    public class TradeService: ITradeService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeService(ITradeRepository tradingRepository)
        {
            _tradeRepository = tradingRepository;
        }

        public async Task<IEnumerable<TradeDto>> GetAllTrade()
        {
            IEnumerable<Trade> trades = await _tradeRepository.GetAllTrade();
            List<TradeDto> tradeDtos = new List<TradeDto>();
            foreach (var dto in trades)
            {
                tradeDtos.Add(MappingTradeForDto(dto));
            }
            return tradeDtos.ToList();
        }

        public async Task<TradeDto?> GetTradeById(int id)
        {
            Trade? trade = await _tradeRepository.GetTradeById(id);
            if (trade == null)
                return null;
            return MappingTradeForDto(trade);
        }

        public async Task<TradeModel?> GetTradeModelById(int id)
        {
            Trade? trade = await _tradeRepository.GetTradeById(id);
            if (trade == null)
                return null;
            return MappingTradeForModel(trade);
        }

        public async Task<ServiceResult<TradeDto>> AddTrade(TradeModel tradeModel, string userName)
        {
            ValidationContext context = new ValidationContext(tradeModel);
            var result = new ServiceResult<TradeDto>();
            if(!Validator.TryValidateObject(tradeModel, context, result.Errors, true))
                return result;

            Trade trade = MappingTradeModelForDatabase(tradeModel, new Trade());
            trade.CreationName = userName;
            trade.CreationDate = DateTime.Now;
            trade.RevisionName = userName;
            trade.RevisionDate = DateTime.Now;

            await _tradeRepository.AddTrade(trade);

            result.Data = MappingTradeForDto(trade);
            return result;

        }

        public async Task<ServiceResult<TradeDto>> UpdateTrade(TradeModel tradeModel, string userName, int id)
        {
            ValidationContext context = new ValidationContext(tradeModel);
            var result = new ServiceResult<TradeDto>();
            if (!Validator.TryValidateObject(tradeModel, context, result.Errors, true))
                return result;

            Trade? findTrade = await _tradeRepository.GetTradeById(id);
            if (findTrade == null)
            {
                result.Errors.Add(new ValidationResult(TradeModelRessources.TradeNotFound));
                return result;
            }

            findTrade = MappingTradeModelForDatabase(tradeModel, findTrade);
            findTrade.RevisionName = userName;
            findTrade.RevisionDate = DateTime.Now;

            await _tradeRepository.UpdateTrade(findTrade);
                
            result.Data = MappingTradeForDto(findTrade);
            return result;
        }

        public async Task<List<ValidationResult>> DeleteTrade(int id)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            Trade? trade = await _tradeRepository.GetTradeById(id);
            if(trade == null)
            {
                result.Add(new ValidationResult(TradeModelRessources.TradeNotFound));
                return result;
            }

            await _tradeRepository.DeleteTrade(trade);
            return result;
        }

        ///////////////////// PRIVATE FUNCTION /////////////////////

        private Trade MappingTradeModelForDatabase(TradeModel tradeModel, Trade trade)
        {
            trade.Account = tradeModel.Account;
            trade.AccountType = tradeModel.AccountType;
            trade.BuyQuantity = tradeModel.BuyQuantity;
            trade.SellQuantity = tradeModel.SellQuantity;
            trade.BuyPrice = tradeModel.BuyPrice;
            trade.SellPrice = tradeModel.SellPrice;
            trade.TradeDate = tradeModel.TradeDate;
            trade.TradeSecurity = tradeModel.TradeSecurity;
            trade.TradeStatus = tradeModel.TradeStatus;
            trade.Trader = tradeModel.Trader;
            trade.Benchmark = tradeModel.Benchmark;
            trade.Book = tradeModel.Book;
            trade.DealName = tradeModel.DealName;
            trade.DealType = tradeModel.DealType;
            trade.SourceListId = tradeModel.SourceListId;
            trade.Side = tradeModel.Side;

            return trade;
        }

        private TradeModel MappingTradeForModel(Trade trade)
        {
            TradeModel tradeModel = new TradeModel
            {
                Account = trade.Account,
                AccountType = trade.AccountType,
                BuyQuantity = trade.BuyQuantity,
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeDate = trade.TradeDate,
                TradeSecurity = trade.TradeSecurity,
                TradeStatus = trade.TradeStatus,
                Trader = trade.Trader,
                Benchmark = trade.Benchmark,
                Book = trade.Book,
                DealName = trade.DealName,
                DealType = trade.DealType,
                SourceListId = trade.SourceListId,
                Side = trade.Side,
            };

            return tradeModel;
        }

        private TradeDto MappingTradeForDto(Trade trade)
        {
            TradeDto tradeDto = new TradeDto
            {
                TradeId = trade.TradeId,
                Account = trade.Account,
                AccountType = trade.AccountType,
                BuyQuantity = trade.BuyQuantity,
                SellQuantity = trade.SellQuantity,
                BuyPrice = trade.BuyPrice,
                SellPrice = trade.SellPrice,
                TradeDate = trade.TradeDate,
                TradeSecurity = trade.TradeSecurity,
                TradeStatus = trade.TradeStatus,
                Trader = trade.Trader,
                Benchmark = trade.Benchmark,
                Book = trade.Book,
                CreationDate = trade.CreationDate,
                CreationName = trade.CreationName,
                RevisionDate = trade.RevisionDate,
                RevisionName = trade.RevisionName,
                DealName = trade.DealName,
                DealType = trade.DealType,
                SourceListId = trade.SourceListId,
                Side = trade.Side,
            };
            return tradeDto;
        }

    }
}
