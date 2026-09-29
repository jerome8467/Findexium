using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Service;
using P7CreateRestApi.Service.Interfaces;
using System.Data;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.CurvePointTest
{
    public class CurvePointServiceTest : IDisposable
    {
        private readonly ICurvePointService _curvePointService;
        private readonly ICurvePointRepository _curvePointRepository;
        private readonly LocalDbContext _dbContext;

        public CurvePointServiceTest()
        {
            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite("Data source=:memory:")
                .Options;
            _dbContext = new LocalDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _dbContext.curvePoints.Add(CreateValidCurvePoint(1, 10));
            _dbContext.curvePoints.Add(CreateValidCurvePoint(2, 20));
            _dbContext.curvePoints.Add(CreateValidCurvePoint(3, 30));
            _dbContext.curvePoints.Add(CreateValidCurvePoint(4, 40));
            _dbContext.SaveChanges();

            _curvePointRepository = new CurvePointRepository(_dbContext);
            _curvePointService = new CurvePointService(_curvePointRepository);
        }

        private CurvePoint CreateValidCurvePoint(int id, byte curveId)
        {
            return new CurvePoint
            {
                Id = id,
                CurveId = curveId
            };
        }

        public void Dispose()
        {
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task Service_GetAllCurvePoint()
        {
            // ACT
            IEnumerable<CurvePoint> curvePoints = await _curvePointService.GetAllCurvePoint();

            // ASSERT
            Assert.Equal(4, curvePoints.Count());
        }

        [Fact]
        public async Task Service_GetCurvePointById()
        {
            // ACT
            CurvePoint? curvePoint = await _curvePointService.GetCurvePointById(1);

            // ASSERT
            Assert.NotNull(curvePoint);
            Assert.Equal(1, curvePoint?.Id);
            Assert.Equal((byte)10, curvePoint?.CurveId);
        }

        [Fact]
        public async Task Service_GetCurvePointModelById()
        {
            // ACT
            CurvePointModel? curvePointModel = await _curvePointService.GetCurvePointModelById(1);

            // ASSERT
            Assert.NotNull(curvePointModel);
            Assert.Equal(10, curvePointModel?.CurveId);
        }

        [Fact]
        public async Task Service_UpdateCurvePoint()
        {
            // ARRANGE
            CurvePointModel curvePointModel = new CurvePointModel
            {
                CurveId = 15,
            };

            // ACT
            ServiceResult<CurvePoint> serviceResult = await _curvePointService.UpdateCurvePoint(curvePointModel, 1);
            CurvePoint curvePoint = await _dbContext.curvePoints
                .AsNoTracking()
                .FirstAsync(i => i.Id == 1);

            // ASSERT
            Assert.NotNull(curvePoint);
            Assert.Equal(1, curvePoint.Id);
            Assert.Equal((byte)15, curvePoint.CurveId);
            Assert.Empty(serviceResult?.Errors);
        }

        [Fact]
        public async Task Service_AddCurvePoint()
        {
            // ARRANGE
            CurvePointModel curvePointModel = new CurvePointModel
            {
                CurveId = 50,
            };

            // ACT
            ServiceResult<CurvePoint> serviceResult = await _curvePointService.AddCurvePoint(curvePointModel);
            CurvePoint? curvePoint = await _dbContext.curvePoints
                .AsNoTracking()
                .OrderBy(i => i.Id).LastAsync();
            int curvePointsCount = await _dbContext.curvePoints.CountAsync();

            // ASSERT
            Assert.NotNull(curvePoint);
            Assert.Equal(5, curvePointsCount);
            Assert.Equal(5, curvePoint.Id);
            Assert.Equal((byte)50, curvePoint.CurveId);
            Assert.Empty(serviceResult?.Errors);
        }

        [Fact]
        public async Task Service_DeleteCurvePoint()
        {
            // ACT
            await _curvePointService.DeleteCurvePoint(2);
            IEnumerable<CurvePoint> curvePoints = await _dbContext.curvePoints
                .ToListAsync();

            // ASSERT

            Assert.Equal(3, curvePoints.Count());
            Assert.Contains(curvePoints, i => i.Id == 1);
            Assert.Contains(curvePoints, i => i.Id == 3);
            Assert.Contains(curvePoints, i => i.Id == 4);
            Assert.DoesNotContain(curvePoints, i => i.Id == 2);
        }

    }
}
