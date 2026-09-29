using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service.Interfaces
{
    public interface IRatingService
    {
        public Task<IEnumerable<Rating>> GetAllRating();
        public Task<Rating?> GetRatingById(int id);
        public Task<RatingModel?> GetRatingModelById(int id);
        public Task<ServiceResult<Rating>> AddRating(RatingModel ratingModel);
        public Task<ServiceResult<Rating>> UpdateRating(RatingModel ratingModel, int id);
        public Task<List<ValidationResult>> DeleteRating(int id);
    }
}
