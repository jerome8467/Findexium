using P7CreateRestApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface ICurvePointRepository
    {
        public Task<IEnumerable<CurvePoint>> GetAllCurvePoint();
        public Task<CurvePoint?> GetCurvePointById(int id);
        public Task AddCurvePoint(CurvePoint curvePoint);
        public Task UpdateCurvePoint(CurvePoint curvePoint);
        public Task DeleteCurvePoint(CurvePoint curvePoint);

    }
}
