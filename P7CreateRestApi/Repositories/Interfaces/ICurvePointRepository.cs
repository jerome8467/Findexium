using P7CreateRestApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface ICurvePointRepository
    {
        public Task<IEnumerable<CurvePoint>> GetAllCurvePoint();
        public Task<CurvePoint?> GetCurvePointById(int id);
        public Task AddCurvePoint(CurvePoint curvePoint);
        public Task<bool> UpdateCurvePoint(CurvePoint curvePoint);
        public Task<bool> DeleteCurvePoint(int id);

    }
}
