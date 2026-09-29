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

        public async Task UpdateCurvePoint(CurvePoint curvePoint)
        {
            _DbContext.Update(curvePoint);
            await _DbContext.SaveChangesAsync();
        }

        public async Task DeleteCurvePoint(CurvePoint curvePoint)
        {
            _DbContext.curvePoints.Remove(curvePoint);
            await _DbContext.SaveChangesAsync();
        }
    }
}
