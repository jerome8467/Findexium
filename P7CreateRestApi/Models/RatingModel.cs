using P7CreateRestApi.Ressource;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models
{
    public class RatingModel
    {
        [Required(ErrorMessageResourceType = typeof(RatingModelRessources),
            ErrorMessageResourceName = "MoodysRatingRequired")]
        public string MoodysRating { get; set; }

        [Required(ErrorMessageResourceType = typeof(RatingModelRessources),
            ErrorMessageResourceName = "SandPRatingRequired")]
        public string SandPRating { get; set; }

        [Required(ErrorMessageResourceType = typeof(RatingModelRessources),
            ErrorMessageResourceName = "FitchRatingRequired")]
        public string FitchRating { get; set; }

        public byte? OrderNumber { get; set; }
    }
}
