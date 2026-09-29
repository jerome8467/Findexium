using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.RatingTest
{
    public class RatingModelValidationTest
    {
        private const string validMoodysRating = "Ok";
        private const string validSandPRating = "Ok";
        private const string validFitchRating = "Ok";
        private const int validOrderNumber = 1;

        private List<string> TryValidateRating(RatingModel ratingModel )
        {

            var context = new ValidationContext(ratingModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(ratingModel, context, result, true);

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
        public void Rating_with_AllEmpty()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = "",
                SandPRating = "",
                FitchRating = ""
            };

            // ACT
            var result = TryValidateRating(ratingModel);

            // ASSERT
            Assert.Contains("MoodysRating", result);
            Assert.Contains("SandPRating", result);
            Assert.Contains("FitchRating", result);
            Assert.Equal(3, result?.Count);
        }

        [Fact]
        public void Rating_with_AllValid()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = validMoodysRating,
                SandPRating = validSandPRating,
                FitchRating = validFitchRating
            };

            // ACT
            var result = TryValidateRating(ratingModel);

            // ASSERT
            Assert.DoesNotContain("MoodysRating", result);
            Assert.DoesNotContain("SandPRating", result);
            Assert.DoesNotContain("FitchRating", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void Rating_with_MoodysRating_Empty()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = "",
                SandPRating = validSandPRating,
                FitchRating = validFitchRating
            };

            // ACT
            var result = TryValidateRating(ratingModel);

            // ASSERT
            Assert.Contains("MoodysRating", result);
            Assert.DoesNotContain("SandPRating", result);
            Assert.DoesNotContain("FitchRating", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Rating_with_SandPRating_Empty()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = validMoodysRating,
                SandPRating = "",
                FitchRating = validFitchRating
            };

            // ACT
            var result = TryValidateRating(ratingModel);

            // ASSERT
            Assert.DoesNotContain("MoodysRating", result);
            Assert.Contains("SandPRating", result);
            Assert.DoesNotContain("FitchRating", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Rating_with_FitchRating_Empty()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = validMoodysRating,
                SandPRating = validSandPRating,
                FitchRating = ""
            };

            // ACT
            var result = TryValidateRating(ratingModel);

            // ASSERT
            Assert.DoesNotContain("MoodysRating", result);
            Assert.DoesNotContain("SandPRating", result);
            Assert.Contains("FitchRating", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Rating_with_OrderNumber_NotRange()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = validMoodysRating,
                SandPRating = validSandPRating,
                FitchRating = validFitchRating,
                OrderNumber = 300
            };

            // ACT
            var result = TryValidateRating(ratingModel);

            // ASSERT
            Assert.DoesNotContain("MoodysRating", result);
            Assert.DoesNotContain("SandPRating", result);
            Assert.DoesNotContain("FitchRating", result);
            Assert.Contains("OrderNumber", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Rating_with_OrderNumber_Valid()
        {
            // ARRANGE
            RatingModel ratingModel = new RatingModel
            {
                MoodysRating = validMoodysRating,
                SandPRating = validSandPRating,
                FitchRating = validFitchRating,
                OrderNumber = validOrderNumber
            };

            // ACT
            var result = TryValidateRating(ratingModel);

            // ASSERT
            Assert.DoesNotContain("MoodysRating", result);
            Assert.DoesNotContain("SandPRating", result);
            Assert.DoesNotContain("FitchRating", result);
            Assert.DoesNotContain("OrderNumber", result);
            Assert.Equal(0, result?.Count);
        }





    }
}
