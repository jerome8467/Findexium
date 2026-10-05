using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Service;
using P7CreateRestApi.Service.Interfaces;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.RatingTest
{
    public class RatingServiceTest : IDisposable
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRatingService _ratingService;
        private readonly LocalDbContext _dbContext;

        public RatingServiceTest()
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite("Data source=:memory:")
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _dbContext.ratings.Add(CreateValidRating(1, "Mood1", "Sand1", "Fitch1", 10));
            _dbContext.ratings.Add(CreateValidRating(2, "Mood2", "Sand2", "Fitch2", 20));
            _dbContext.ratings.Add(CreateValidRating(3, "Mood3", "Sand3", "Fitch3", 30));
            _dbContext.ratings.Add(CreateValidRating(4, "Mood4", "Sand4", "Fitch4", 40));
            _dbContext.SaveChanges();

            _ratingRepository = new RatingRepository(_dbContext);
            _ratingService = new RatingService(_ratingRepository);
        }

        private Rating CreateValidRating(int id, string moodysRating, string SandPRating, string FitchRating, int OrderNumber)
        {
            return new Rating
            {
                Id = id,
                MoodysRating = moodysRating,
                SandPRating = SandPRating,
                FitchRating = FitchRating,
                OrderNumber = (byte)OrderNumber
            };
        }

        public void Dispose()
        {
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task Service_GetAllRating()
        {
            // ACT
            IEnumerable<RatingDto> ratings = await _ratingService.GetAllRating();

            // ASSERT
            Assert.Equal(4, ratings.Count());
        }

        [Fact]
        public async Task Service_GetRatingById()
        {
            // ACT
            RatingDto? ratingDto = await _ratingService.GetRatingById(1);

            // ASSERT
            Assert.NotNull(ratingDto);
            Assert.Equal(1, ratingDto?.Id);
            Assert.Equal((byte)10, ratingDto?.OrderNumber);
        }

        [Fact]
        public async Task Service_GetRatingModelById()
        {
            // ACT
            RatingModel? ratingModel = await _ratingService.GetRatingModelById(1);

            // ASSERT
            Assert.NotNull(ratingModel);
            Assert.Equal((byte)10, ratingModel?.OrderNumber);
        }

        [Fact]
        public async Task Service_UpdateRating()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = "Mood1.5",
                SandPRating = "Sand1.5",
                FitchRating = "Fitch1.5",
                OrderNumber = (byte)11
            };

            // ACT
            ServiceResult<RatingDto> serviceResult = await _ratingService.UpdateRating(ratingModel, 1);
            Rating rating = await _dbContext.ratings
                .AsNoTracking()
                .FirstAsync(i => i.Id == 1);

            // ASSERT
            Assert.NotNull(rating);
            Assert.Equal(1, rating.Id);
            Assert.Equal("Mood1.5", rating.MoodysRating);
            Assert.Equal("Sand1.5", rating.SandPRating);
            Assert.Equal("Fitch1.5", rating.FitchRating);
            Assert.Equal((byte)11, rating.OrderNumber);
            Assert.Empty(serviceResult?.Errors);
        }

        [Fact]
        public async Task Service_AddRating()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = "Mood5",
                SandPRating = "Sand5",
                FitchRating = "Fitch5",
                OrderNumber = (byte)50
            };

            // ACT
            ServiceResult<RatingDto> serviceResult = await _ratingService.AddRating(ratingModel);
            Rating? rating = await _dbContext.ratings
                .AsNoTracking()
                .OrderBy(i => i.Id).LastAsync();
            int ratingCount = await _dbContext.ratings.CountAsync();

            // ASSERT
            Assert.Equal(5, ratingCount);
            Assert.Equal(5, rating.Id);
            Assert.Equal("Mood5", rating.MoodysRating);
            Assert.Equal("Sand5", rating.SandPRating);
            Assert.Equal("Fitch5", rating.FitchRating);
            Assert.Equal((byte)50, rating.OrderNumber);
            Assert.Empty(serviceResult?.Errors);
        }

        [Fact]
        public async Task Service_DeleteRating()
        {
            // ACT
            await _ratingService.DeleteRating(2);
            IEnumerable<Rating> ratings = await _dbContext.ratings.ToListAsync();

            // ASSERT
            Assert.Equal(3, ratings.Count());
            Assert.Contains(ratings, i => i.Id == 1);
            Assert.Contains(ratings, i => i.Id == 3);
            Assert.Contains(ratings, i => i.Id == 4);
            Assert.DoesNotContain(ratings, i => i.Id == 2);
        }

    }
}
