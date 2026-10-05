using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<CurvePointDto>> GetAllCurvePoint()
        {
            IEnumerable<CurvePoint> curvePoints = await _curvePointRepository.GetAllCurvePoint();
            List<CurvePointDto> curvePointDtos = new List<CurvePointDto>();
            foreach (var dto in curvePoints)
            {
                curvePointDtos.Add(MappingCurvePointForDto(dto));
            }

            return curvePointDtos.ToList();
        }

        public async Task<CurvePointDto?> GetCurvePointById(int id)
        {
            CurvePoint? curvePoint = await _curvePointRepository.GetCurvePointById(id);
            if (curvePoint == null)
                return null;
            return MappingCurvePointForDto(curvePoint);
        }

        public async Task<CurvePointModel?> GetCurvePointModelById(int id)
        {
            CurvePoint? curvePoint = await _curvePointRepository.GetCurvePointById(id);
            if (curvePoint == null)
                return null;
            return MappingCurvePointForModel(curvePoint);
        }

        public async Task<ServiceResult<CurvePointDto>> AddCurvePoint(CurvePointModel curvePointModel)
        {
            var result = new ServiceResult<CurvePointDto>();
            ValidationContext context = new ValidationContext(curvePointModel);
            if (!Validator.TryValidateObject(curvePointModel, context, result.Errors, true))
                return result;

            CurvePoint curvePoint = MappingCurvePointModelForDatabase(curvePointModel, new CurvePoint());
            curvePoint.CreationDate = DateTime.Now;

            await _curvePointRepository.AddCurvePoint(curvePoint);

            result.Data = MappingCurvePointForDto(curvePoint); 
            return result;
        }

        public async Task<ServiceResult<CurvePointDto>> UpdateCurvePoint(CurvePointModel curvePointModel, int id)
        {
            var result = new ServiceResult<CurvePointDto>();
            ValidationContext context = new ValidationContext(curvePointModel);
            if (!Validator.TryValidateObject(curvePointModel, context, result.Errors, true))
                return result;

            CurvePoint? findCurvePoint = await _curvePointRepository.GetCurvePointById(id);
            if(findCurvePoint == null)
            {
                result.Errors.Add(new ValidationResult(CurvePointModelRessources.CurvePointNotFound));
                return result;
            }

            findCurvePoint = MappingCurvePointModelForDatabase(curvePointModel, findCurvePoint);

            await _curvePointRepository.UpdateCurvePoint(findCurvePoint);

            result.Data = MappingCurvePointForDto(findCurvePoint);
            return result;
        }

        public async Task<List<ValidationResult>> DeleteCurvePoint(int id)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            CurvePoint? findCurvePoint = await _curvePointRepository.GetCurvePointById(id);
            if (findCurvePoint == null)
            {
                result.Add(new ValidationResult(CurvePointModelRessources.CurvePointNotFound));
                return result;
            }

            await _curvePointRepository.DeleteCurvePoint(findCurvePoint);
            return result;
        }

        private CurvePoint MappingCurvePointModelForDatabase(CurvePointModel curvePointModel, CurvePoint curvePoint)
        {

            curvePoint.CurveId = (byte?)curvePointModel.CurveId;
            curvePoint.AsOfDate = curvePointModel.AsOfDate;
            curvePoint.Term = curvePointModel.Term;
            curvePoint.CurvePointValue = curvePointModel.CurvePointValue;

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

        private CurvePointDto MappingCurvePointForDto(CurvePoint curvePoint)
        {
            CurvePointDto curvePointDto = new CurvePointDto
            {
                Id = curvePoint.Id,
                CurveId = curvePoint.CurveId,
                AsOfDate = curvePoint.AsOfDate,
                Term = curvePoint.Term,
                CurvePointValue = curvePoint.CurvePointValue,
                CreationDate = curvePoint.CreationDate,
            };
            return curvePointDto;
        }

    }
}
