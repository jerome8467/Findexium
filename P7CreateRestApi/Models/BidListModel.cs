using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models
{
    public class BidListModel
    {
        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "AccountRequired")]
        public string Account { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "BidTypeRequired")]
        public string BidType { get; set; }

        public double? BidQuantity { get; set; }
        public double? AskQuantity { get; set; }
        public double? Bid { get; set; }
        public double? Ask { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "BenchmarkRequired")]
        public string Benchmark { get; set; }

        public DateTime? BidListDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "CommentaryRequired")]
        public string Commentary { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "BidSecurityRequired")]
        public string BidSecurity { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "BidStatusRequired")]
        public string BidStatus { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "TraderRequired")]
        public string Trader { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "BookRequired")]
        public string Book { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "DealNameRequired")]
        public string DealName { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "DealTypeRequired")]
        public string DealType { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "SourceListIdRequired")]
        public string SourceListId { get; set; }

        [Required(ErrorMessageResourceType = typeof(BidListModelRessources),
            ErrorMessageResourceName = "SideRequired")]
        public string Side { get; set; }
    }
}
