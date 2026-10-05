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

        public async Task<IEnumerable<RatingDto>> GetAllRating()
        {
            IEnumerable<Rating> ratings = await _ratingRepository.GetAllRating();
            List<RatingDto> ratingDtos = new List<RatingDto>();
            foreach (var dto in ratings)
            {
                ratingDtos.Add(MappingRatingForDto(dto));
            }
            return ratingDtos.ToList();
        }

        public async Task<RatingDto?> GetRatingById(int id)
        {
            Rating? rating = await _ratingRepository.GetRatingById(id);
            if (rating == null)
                return null;
            return MappingRatingForDto(rating);
        }

        public async Task<RatingModel?> GetRatingModelById(int id)
        {
            Rating? rating = await _ratingRepository.GetRatingById(id);
            if (rating == null)
                return null;
            return MappingRatingForModel(rating);
        }

        public async Task<ServiceResult<RatingDto>> AddRating(RatingModel ratingModel)
        {
            var result =  new ServiceResult<RatingDto>();
            ValidationContext context = new ValidationContext(ratingModel);
            if (!Validator.TryValidateObject(ratingModel, context, result.Errors, true))
                return result;

            Rating rating = MappingRatingModelForDatabase(ratingModel, new Rating());

            await _ratingRepository.AddRating(rating);

            result.Data = MappingRatingForDto(rating);
            return result;
        }

        public async Task<ServiceResult<RatingDto>> UpdateRating(RatingModel ratingModel, int id)
        {
            var result = new ServiceResult<RatingDto>();
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

            result.Data = MappingRatingForDto(findRating);
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

        private RatingDto MappingRatingForDto(Rating rating)
        {
            RatingDto ratingDto = new RatingDto
            {
                Id = rating.Id,
                MoodysRating = rating.MoodysRating,
                SandPRating = rating.SandPRating,
                FitchRating = rating.FitchRating,
                OrderNumber = rating.OrderNumber
            };
            return ratingDto;
        }

    }
}
