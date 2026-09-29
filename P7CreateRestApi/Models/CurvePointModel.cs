using P7CreateRestApi.Attribute;
using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models
{
    public class CurvePointModel
    {
        [Required(ErrorMessageResourceType = typeof(CurvePointModelRessources),
            ErrorMessageResourceName = "CurveIdRequired")]
        [Range(0, 255, ErrorMessageResourceType = typeof(CurvePointModelRessources),
            ErrorMessageResourceName = "CurveIdRange")]
        public int? CurveId { get; set; }

        public DateTime? AsOfDate { get; set; }
        public double? Term { get; set; }
        public double? CurvePointValue { get; set; }
    }
}
