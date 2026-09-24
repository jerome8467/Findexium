using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service.Interfaces
{
    public interface ICurvePointService
    {
        public Task<IEnumerable<CurvePoint>> GetAllCurvePoint();
        public Task<CurvePoint?> GetCurvePointById(int id);
        public Task<CurvePointModel?> GetCurvePointModelById(int id);
        public Task<ServiceResult<CurvePoint>> AddCurvePoint(CurvePointModel curvePointModel);
        public Task<ServiceResult<CurvePoint>> UpdateCurvePoint(CurvePointModel curvePointModel, int id);
        public Task<bool> DeleteCurvePoint(int id);
    }
}
