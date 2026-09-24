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

        public async Task<IEnumerable<Trade>> GetAllTrade()
        {
            return await _tradeRepository.GetAllTrade();
        }

        public async Task<Trade?> GetTradeById(int id)
        {
            return await _tradeRepository.GetTradeById(id);
        }

        public async Task<TradeModel?> GetTradeModelById(int id)
        {
            Trade? trade = await _tradeRepository.GetTradeById(id);
            if (trade == null)
                return null;
            return MappingTradeForModel(trade);
        }

        public async Task<ServiceResult<Trade>> AddTrade(TradeModel tradeModel, string userName)
        {
            ValidationContext context = new ValidationContext(tradeModel);
            var result = new ServiceResult<Trade>();
            if(!Validator.TryValidateObject(tradeModel, context, result.Errors, true))
                return result;

            Trade trade = MappingTradeModelForDatabase(tradeModel);
            trade.CreationName = userName;
            trade.CreationDate = DateTime.Now;
            trade.RevisionName = userName;
            trade.RevisionDate = DateTime.Now;

            await _tradeRepository.AddTrade(trade);

            result.Data = trade;
            return result;

        }

        public async Task<ServiceResult<Trade>> UpdateTrade(TradeModel tradeModel, string userName, int id)
        {
            ValidationContext context = new ValidationContext(tradeModel);
            var result = new ServiceResult<Trade>();
            if (!Validator.TryValidateObject(tradeModel, context, result.Errors, true))
                return result;

            Trade trade = MappingTradeModelForDatabase(tradeModel);
            trade.RevisionName = userName;
            trade.RevisionDate = DateTime.Now;

            bool success = await _tradeRepository.UpdateTrade(trade);
            if (!success)
            {
                result.Errors.Add(new ValidationResult(TradeModelRessources.TradeNotFound));
                return result;
            }
                
            result.Data = trade;
            return result;
        }

        public async Task<bool> DeleteTrade(int id)
        {
            return await _tradeRepository.DeleteTrade(id);
        }

        private Trade MappingTradeModelForDatabase(TradeModel tradeModel)
        {
            Trade trade = new Trade
            {
                Account = tradeModel.Account,
                AccountType = tradeModel.AccountType,
                BuyQuantity = tradeModel.BuyQuantity,
                SellQuantity = tradeModel.SellQuantity,
                BuyPrice = tradeModel.BuyPrice,
                SellPrice = tradeModel.SellPrice,
                TradeDate = tradeModel.TradeDate,
                TradeSecurity = tradeModel.TradeSecurity,
                TradeStatus = tradeModel.TradeStatus,
                Trader = tradeModel.Trader,
                Benchmark = tradeModel.Benchmark,
                Book = tradeModel.Book,
                DealName = tradeModel.DealName,
                DealType = tradeModel.DealType,
                SourceListId = tradeModel.SourceListId,
                Side = tradeModel.Side,
            };

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

    }
}
