using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories.Interfaces;

namespace P7CreateRestApi.Repositories
{
    public class CurvePointRepository:ICurvePointRepository
    {
        private readonly LocalDbContext _DbContext;

        public CurvePointRepository(LocalDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<IEnumerable<CurvePoint>> GetAllCurvePoint()
        {
            return await _DbContext.curvePoints.ToListAsync();
        }

        public async Task<CurvePoint?> GetCurvePointById(int id)
        {
            return await _DbContext.curvePoints.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCurvePoint(CurvePoint curvePoint)
        {
            _DbContext.curvePoints.Add(curvePoint);
            await _DbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateCurvePoint(CurvePoint curvePoint)
        {
            CurvePoint? findCurvePoint = await GetCurvePointById(curvePoint.Id);
            if (findCurvePoint == null)
                return false;

            _DbContext.Entry(findCurvePoint).CurrentValues.SetValues(curvePoint);
            await _DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCurvePoint(int id)
        {
            CurvePoint? findCurvePoint = await GetCurvePointById(id);
            if (findCurvePoint == null)
                return false;

            _DbContext.curvePoints.Remove(findCurvePoint);
            await _DbContext.SaveChangesAsync();
            return true;
        }


    }
}
