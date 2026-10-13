using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Models.UserModel;
using System.Net;
using System.Net.Http.Json;
using Test_P7CreateRestApi.Integration.Outils;
using Xunit;

namespace Test_P7CreateRestApi.Integration.RatingTest
{
    public class RatingControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly LocalDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;

        public RatingControllerTest()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite(_connection)
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Users.Add(CreateUser.NewValidUser(1, "AdminTest", nameof(UserRole.Admin)));

            _dbContext.ratings.Add(CreateEntity.NewValidRating(1, "Moodys1", "SandP1", "Fitch1", 10));
            _dbContext.ratings.Add(CreateEntity.NewValidRating(2, "Moodys2", "SandP2", "Fitch2", 20));
            _dbContext.ratings.Add(CreateEntity.NewValidRating(3, "Moodys3", "SandP3", "Fitch3", 30));
            _dbContext.ratings.Add(CreateEntity.NewValidRating(4, "Moodys4", "SandP4", "Fitch4", 40));
            _dbContext.SaveChanges();

            _factory = new CustomWebApplicationFactory(_connection);
        }

        public void Dispose()
        {
            _factory.Dispose();
            _dbContext.Dispose();
            _connection.Dispose();
        }

        [Fact]
        public async Task Controller_GetAllRating_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Rating/List");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetAllRating_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Rating/List");
            List<RatingDto>? ratings = await response.Content.ReadFromJsonAsync<List<RatingDto>>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(ratings);
            Assert.Equal(4, ratings?.Count);
            Assert.Contains(ratings, r => r.Id == 1 && r.MoodysRating == "Moodys1");
        }

        [Fact]
        public async Task Controller_GetRatingById_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Rating/Details/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetRatingById_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Rating/Details/1");
            RatingDto? rating = await response.Content.ReadFromJsonAsync<RatingDto>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(rating);
            Assert.Equal(1, rating?.Id);
            Assert.Equal("Moodys1", rating?.MoodysRating);
        }

        [Fact]
        public async Task Controller_GetRatingById_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Rating/Details/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewRating_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            RatingModel ratingModel = CreateModel.NewValidRatingModel("Moodys5", 50);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Rating/Creation", ratingModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewRating_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RatingModel ratingModel = CreateModel.NewValidRatingModel("Moodys5", 50);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Rating/Creation", ratingModel);
            RatingDto? rating = await response.Content.ReadFromJsonAsync<RatingDto>();
            int ratingCount = await _dbContext.ratings.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(rating);
            Assert.Equal("Moodys5", rating?.MoodysRating);
            Assert.Equal(5, ratingCount);
        }

        [Fact]
        public async Task Controller_AddNewRating_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RatingModel ratingModel = CreateModel.NewValidRatingModel("", 50);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Rating/Creation", ratingModel);
            int ratingCount = await _dbContext.ratings.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(4, ratingCount);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Rating/FormUpdate/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Rating/FormUpdate/1");
            RatingModel? ratingModel = await response.Content.ReadFromJsonAsync<RatingModel>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(ratingModel);
            Assert.Equal("Moodys1", ratingModel?.MoodysRating);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Rating/FormUpdate/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateRating_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            RatingModel ratingModel = CreateModel.NewValidRatingModel("Moodys1Update", 15);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Rating/Modification/1", ratingModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateRating_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RatingModel ratingModel = CreateModel.NewValidRatingModel("Moodys1Update", 15);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Rating/Modification/1", ratingModel);
            Rating rating = await _dbContext.ratings
                .AsNoTracking()
                .FirstAsync(r => r.Id == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Moodys1Update", rating.MoodysRating);
            Assert.Equal(15, (int?)rating.OrderNumber);
        }

        [Fact]
        public async Task Controller_UpdateRating_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RatingModel ratingModel = CreateModel.NewValidRatingModel("", 15);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Rating/Modification/1", ratingModel);
            Rating rating = await _dbContext.ratings
                .AsNoTracking()
                .FirstAsync(r => r.Id == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Moodys1", rating.MoodysRating);
        }

        [Fact]
        public async Task Controller_UpdateRating_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            RatingModel ratingModel = CreateModel.NewValidRatingModel("Moodys1Update", 15);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Rating/Modification/999", ratingModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Controller_DeleteRating_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Rating/Removal/1");
            int ratingCount = await _dbContext.ratings.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal(4, ratingCount);
        }

        [Fact]
        public async Task Controller_DeleteRating_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Rating/Removal/1");
            IEnumerable<Rating> ratings = await _dbContext.ratings
                .AsNoTracking()
                .ToListAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, ratings.Count());
            Assert.DoesNotContain(ratings, r => r.Id == 1);
        }

        [Fact]
        public async Task Controller_DeleteRating_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Rating/Removal/999");
            int ratingCount = await _dbContext.ratings.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(4, ratingCount);
        }
    }
}