using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service.Interfaces
{
    public interface ICurvePointService
    {
        public Task<IEnumerable<CurvePointDto>> GetAllCurvePoint();
        public Task<CurvePointDto?> GetCurvePointById(int id);
        public Task<CurvePointModel?> GetCurvePointModelById(int id);
        public Task<ServiceResult<CurvePointDto>> AddCurvePoint(CurvePointModel curvePointModel);
        public Task<ServiceResult<CurvePointDto>> UpdateCurvePoint(CurvePointModel curvePointModel, int id);
        public Task<List<ValidationResult>> DeleteCurvePoint(int id);
    }
}
