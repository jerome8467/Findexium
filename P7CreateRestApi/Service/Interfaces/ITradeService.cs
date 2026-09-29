using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service.Interfaces
{
    public interface ITradeService
    {
        public Task<IEnumerable<Trade>> GetAllTrade();
        public Task<Trade?> GetTradeById(int id);
        public Task<TradeModel?> GetTradeModelById(int id);
        public Task<ServiceResult<Trade>> AddTrade(TradeModel tradeModel, string userName);
        public Task<ServiceResult<Trade>> UpdateTrade(TradeModel tradeModel, string userName, int id);
        public Task<List<ValidationResult>> DeleteTrade(int id);
    }
}
