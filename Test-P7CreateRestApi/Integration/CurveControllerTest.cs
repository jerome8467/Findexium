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

namespace Test_P7CreateRestApi.Integration.CurvePointTest
{
    public class CurveControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly LocalDbContext _dbContext;
        private readonly CustomWebApplicationFactory _factory;

        public CurveControllerTest()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite(_connection)
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.EnsureCreated();

            _dbContext.Users.Add(CreateUser.NewValidUser(1, "AdminTest", nameof(UserRole.Admin)));

            _dbContext.curvePoints.Add(CreateEntity.NewValidCurvePoint(1, 10));
            _dbContext.curvePoints.Add(CreateEntity.NewValidCurvePoint(2, 20));
            _dbContext.curvePoints.Add(CreateEntity.NewValidCurvePoint(3, 30));
            _dbContext.curvePoints.Add(CreateEntity.NewValidCurvePoint(4, 40));
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
        public async Task Controller_GetAllCurvePoint_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Curve/List");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetAllCurvePoint_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Curve/List");
            List<CurvePointDto>? curvePoints = await response.Content.ReadFromJsonAsync<List<CurvePointDto>>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(curvePoints);
            Assert.Equal(4, curvePoints?.Count);
            Assert.Contains(curvePoints, c => c.Id == 1 && (int?)c.CurveId == 10);
        }

        [Fact]
        public async Task Controller_GetCurvePointById_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Curve/Details/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_GetCurvePointById_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Curve/Details/1");
            CurvePointDto? curvePoint = await response.Content.ReadFromJsonAsync<CurvePointDto>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(curvePoint);
            Assert.Equal(1, curvePoint?.Id);
            Assert.Equal(10, (int?)curvePoint?.CurveId);
        }

        [Fact]
        public async Task Controller_GetCurvePointById_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Curve/Details/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewCurvePoint_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            CurvePointModel curvePointModel = CreateModel.NewValidCurvePointModel(15);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Curve/Creation", curvePointModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_AddNewCurvePoint_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            CurvePointModel curvePointModel = CreateModel.NewValidCurvePointModel(15);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Curve/Creation", curvePointModel);
            CurvePointDto? curvePoint = await response.Content.ReadFromJsonAsync<CurvePointDto>();
            int curvePointCount = await _dbContext.curvePoints.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(curvePoint);
            Assert.Equal(15, (int?)curvePoint?.CurveId);
            Assert.Equal(5, curvePointCount);
        }

        [Fact]
        public async Task Controller_AddNewCurvePoint_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            CurvePointModel curvePointModel = CreateModel.NewValidCurvePointModel(300);

            // ACT
            HttpResponseMessage response = await client.PostAsJsonAsync("/Curve/Creation", curvePointModel);
            int curvePointCount = await _dbContext.curvePoints.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(4, curvePointCount);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Curve/FormUpdate/1");

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Curve/FormUpdate/1");
            CurvePointModel? curvePointModel = await response.Content.ReadFromJsonAsync<CurvePointModel>();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(curvePointModel);
            Assert.Equal(10, curvePointModel?.CurveId);
        }

        [Fact]
        public async Task Controller_ShowUpdateForm_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.GetAsync("/Curve/FormUpdate/999");

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateCurvePoint_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();
            CurvePointModel curvePointModel = CreateModel.NewValidCurvePointModel(15);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Curve/Modification/1", curvePointModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Controller_UpdateCurvePoint_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            CurvePointModel curvePointModel = CreateModel.NewValidCurvePointModel(15);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Curve/Modification/1", curvePointModel);
            CurvePoint curvePoint = await _dbContext.curvePoints
                .AsNoTracking()
                .FirstAsync(c => c.Id == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(15, (int?)curvePoint.CurveId);
        }

        [Fact]
        public async Task Controller_UpdateCurvePoint_InvalidModel()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            CurvePointModel curvePointModel = CreateModel.NewValidCurvePointModel(300);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Curve/Modification/1", curvePointModel);
            CurvePoint curvePoint = await _dbContext.curvePoints
                .AsNoTracking()
                .FirstAsync(c => c.Id == 1);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(10, (int?)curvePoint.CurveId);
        }

        [Fact]
        public async Task Controller_UpdateCurvePoint_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");
            CurvePointModel curvePointModel = CreateModel.NewValidCurvePointModel(15);

            // ACT
            HttpResponseMessage response = await client.PutAsJsonAsync("/Curve/Modification/999", curvePointModel);

            // ASSERT
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Controller_DeleteCurvePoint_WithoutToken()
        {
            // ARRANGE
            HttpClient client = _factory.CreateClient();

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Curve/Removal/1");
            int curvePointCount = await _dbContext.curvePoints.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Equal(4, curvePointCount);
        }

        [Fact]
        public async Task Controller_DeleteCurvePoint_WithToken()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Curve/Removal/1");
            IEnumerable<CurvePoint> curvePoints = await _dbContext.curvePoints
                .AsNoTracking()
                .ToListAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(3, curvePoints.Count());
            Assert.DoesNotContain(curvePoints, c => c.Id == 1);
        }

        [Fact]
        public async Task Controller_DeleteCurvePoint_NotFound()
        {
            // ARRANGE
            HttpClient client = await _factory.CreateClientWithTokenAsync("AdminTest", "Password1!");

            // ACT
            HttpResponseMessage response = await client.DeleteAsync("/Curve/Removal/999");
            int curvePointCount = await _dbContext.curvePoints.CountAsync();

            // ASSERT
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(4, curvePointCount);
        }
    }
}