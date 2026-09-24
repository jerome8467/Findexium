using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models
{
    public class TradeModel
    {
        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "AccountRequired")]
        public string Account { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "AccountTypeRequired")]
        public string AccountType { get; set; }

        public double? BuyQuantity { get; set; }
        public double? SellQuantity { get; set; }
        public double? BuyPrice { get; set; }
        public double? SellPrice { get; set; }
        public DateTime? TradeDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "TradeSecurityRequired")]
        public string TradeSecurity { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "TradeStatusRequired")]
        public string TradeStatus { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "TraderRequired")]
        public string Trader { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "BenchmarkRequired")]
        public string Benchmark { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "BookRequired")]
        public string Book { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "DealNameRequired")]
        public string DealName { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "DealTypeRequired")]
        public string DealType { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "SourceListIdRequired")]
        public string SourceListId { get; set; }

        [Required(ErrorMessageResourceType = typeof(TradeModelRessources),
            ErrorMessageResourceName = "SideRequired")]
        public string Side { get; set; }
    }
}
