using Microsoft.AspNetCore.Mvc;
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
            return await _ratingRepository.GetRatingById(id);
        }

        public async Task<RatingModel?> GetRatingModelById(int id)
        {
            Rating? rating = await _ratingRepository.GetRatingById(id);
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

            Rating rating = MappingRatingModelForDatabase(ratingModel, new Rating());

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

            Rating? findRating = await _ratingRepository.GetRatingById(id);
            if(findRating == null)
            {
                result.Errors.Add(new ValidationResult(RatingModelRessources.RatingNotFound));
                return result;
            }

            findRating = MappingRatingModelForDatabase(ratingModel, findRating);

            await _ratingRepository.UpdateRating(findRating);

            result.Data = findRating;
            return result;
        }

        public async Task<List<ValidationResult>> DeleteRating(int id)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            Rating? findRating = await _ratingRepository.GetRatingById(id);
            if (findRating == null)
            {
                result.Add(new ValidationResult(RatingModelRessources.RatingNotFound));
                return result;
            }
            await _ratingRepository.DeleteRating(findRating);
            return result;
        }

        private Rating MappingRatingModelForDatabase(RatingModel ratingModel, Rating rating)
        {
            rating.MoodysRating = ratingModel.MoodysRating;
            rating.SandPRating = ratingModel.SandPRating;
            rating.FitchRating = ratingModel.FitchRating;
            rating.OrderNumber = (byte?)ratingModel.OrderNumber;

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
