using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.CurvePointTest
{
    public class CurvePointModelValidationTest
    {
        private const byte validCurveId = 1;

        private List<string> TryValidateCurvePoint(CurvePointModel curvePointModel)
        {

            var context = new ValidationContext(curvePointModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(curvePointModel, context, result, true);

            List<string> memberNames = new List<string>();
            foreach (ValidationResult error in result)
            {
                foreach (string memberName in error.MemberNames)
                {
                    memberNames.Add(memberName);
                }
            }
            return memberNames;
        }

        [Fact]
        public void CurvePoint_with_CurveId_Empty()
        {
            // ARRANGE
            CurvePointModel curvePointModel = new CurvePointModel
            {
                CurveId = null
            };

            // ACT
            var result = TryValidateCurvePoint(curvePointModel);

            // ASSERT
            Assert.Contains("CurveId", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void CurvePoint_with_CurveId_NotRange()
        {
            // ARRANGE
            CurvePointModel curvePointModel = new CurvePointModel
            {
                CurveId = 300
            };

            // ACT
            var result = TryValidateCurvePoint(curvePointModel);

            // ASSERT
            Assert.Contains("CurveId", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void CurvePoint_with_CurveId_Valid()
        {
            // ARRANGE
            CurvePointModel curvePointModel = new CurvePointModel
            {
                CurveId = validCurveId
            };

            // ACT
            var result = TryValidateCurvePoint(curvePointModel);

            // ASSERT
            Assert.DoesNotContain("CurveId", result);
            Assert.Equal(0, result?.Count);
        }


    }
}
