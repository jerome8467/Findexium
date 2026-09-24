using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Ressource;
using P7CreateRestApi.Service.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Service
{
    public class CurvePointService : ICurvePointService
    {
        private readonly ICurvePointRepository _curvePointRepository;

        public CurvePointService(ICurvePointRepository curvePointRepository)
        {
            _curvePointRepository = curvePointRepository;
        }

        public async Task<IEnumerable<CurvePoint>> GetAllCurvePoint()
        {
            return await _curvePointRepository.GetAllCurvePoint();
        }

        public async Task<CurvePoint?> GetCurvePointById(int id)
        {
            return await _curvePointRepository.GetCurvePointById(id);
        }

        public async Task<CurvePointModel?> GetCurvePointModelById(int id)
        {
            CurvePoint? curvePoint = await _curvePointRepository.GetCurvePointById(id);
            if (curvePoint == null)
                return null;
            return MappingCurvePointForModel(curvePoint);
        }

        public async Task<ServiceResult<CurvePoint>> AddCurvePoint(CurvePointModel curvePointModel)
        {
            var result = new ServiceResult<CurvePoint>();
            ValidationContext context = new ValidationContext(curvePointModel);
            if (!Validator.TryValidateObject(curvePointModel, context, result.Errors, true))
                return result;

            CurvePoint curvePoint = MappingCurvePointModelForDatabase(curvePointModel);
            curvePoint.CreationDate = DateTime.Now;

            await _curvePointRepository.AddCurvePoint(curvePoint);

            result.Data = curvePoint; 
            return result;
        }

        public async Task<ServiceResult<CurvePoint>> UpdateCurvePoint(CurvePointModel curvePointModel, int id)
        {
            var result = new ServiceResult<CurvePoint>();
            ValidationContext context = new ValidationContext(curvePointModel);
            if (!Validator.TryValidateObject(curvePointModel, context, result.Errors, true))
                return result;

            CurvePoint curvePoint = MappingCurvePointModelForDatabase(curvePointModel);
            curvePoint.Id = id;
            curvePoint.CreationDate = DateTime.Now;

            bool succes = await _curvePointRepository.UpdateCurvePoint(curvePoint);
            if(!succes)
            {
                result.Errors.Add(new ValidationResult(CurvePointModelRessources.CurvePointNotFound));
                return result;   

            }

            result.Data = curvePoint;
            return result;
        }

        public async Task<bool> DeleteCurvePoint(int id)
        {
            return await _curvePointRepository.DeleteCurvePoint(id);
        }

        private CurvePoint MappingCurvePointModelForDatabase(CurvePointModel curvePointModel)
        {
            CurvePoint curvePoint = new CurvePoint
            {
                CurveId = curvePointModel.CurveId,
                AsOfDate = curvePointModel.AsOfDate,
                Term = curvePointModel.Term,
                CurvePointValue = curvePointModel.CurvePointValue
            };

            return curvePoint;
        }

        private CurvePointModel MappingCurvePointForModel(CurvePoint curvePoint)
        {
            CurvePointModel curvePointModel = new CurvePointModel
            {
                CurveId = curvePoint.CurveId,
                AsOfDate = curvePoint.AsOfDate,
                Term = curvePoint.Term,
                CurvePointValue = curvePoint.CurvePointValue
            };

            return curvePointModel;
        }

    }
}
