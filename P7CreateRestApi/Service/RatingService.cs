using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service
{
    public class RatingService: IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<IEnumerable<Rating>> GetAllRating()
        {
            return await _ratingRepository.GetAllRating();
        }

        public async Task<Rating?> GetRatingById(int id)
        {
            return await GetRatingById(id);
        }

        public async Task<RatingModel?> GetRatingModelById(int id)
        {
            Rating? rating = await GetRatingById(id);
            if (rating == null)
                return null;
            return MappingRatingForModel(rating);
        }

        public async Task<ServiceResult<Rating>> AddRating(RatingModel ratingModel)
        {
            var result =  new ServiceResult<Rating>();
            ValidationContext context = new ValidationContext(ratingModel);
            if (!Validator.TryValidateObject(ratingModel, context, result.Errors, true))
                return result;

            Rating rating = MappingRatingModelForDatabase(ratingModel);

            await _ratingRepository.AddRating(rating);

            result.Data = rating;
            return result;
        }

        public async Task<ServiceResult<Rating>> UpdateRating(RatingModel ratingModel, int id)
        {
            var result = new ServiceResult<Rating>();
            ValidationContext context = new ValidationContext(ratingModel);
            if (!Validator.TryValidateObject(ratingModel, context, result.Errors, true))
                return result;

            Rating rating = MappingRatingModelForDatabase(ratingModel);
            rating.Id = id;

            bool succes = await _ratingRepository.UpdateRating(rating);
            if (!succes)
            {
                result.Errors.Add(new ValidationResult(RatingModelRessources.RatingNotFound));
                return result;
            }

            result.Data = rating;
            return result;
        }

        public async Task<bool> DeleteRating(int id)
        {
            return await _ratingRepository.DeleteRating(id);
        }

        private Rating MappingRatingModelForDatabase(RatingModel ratingModel)
        {
            Rating rating = new Rating
            {
                MoodysRating = ratingModel.MoodysRating,
                SandPRating = ratingModel.SandPRating,
                FitchRating = ratingModel.FitchRating,
                OrderNumber = ratingModel.OrderNumber
            };

            return rating;
        }

        private RatingModel MappingRatingForModel(Rating rating)
        {
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = rating.MoodysRating,
                SandPRating = rating.SandPRating,
                FitchRating = rating.FitchRating,
                OrderNumber = rating.OrderNumber
            };

            return ratingModel;
        }


    }
}
