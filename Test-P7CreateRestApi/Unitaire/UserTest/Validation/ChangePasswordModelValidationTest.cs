using P7CreateRestApi.Models.UserModel;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.UserTest.Validation
{
    public class ChangePasswordModelValidationTest
    {
        private const string validOldPassword= "Password@1";
        private const string validNewPassword = "Password@2";

        private List<string> TryValidateChangePassword(ChangePasswordModel changePasswordModel)
        {

            var context = new ValidationContext(changePasswordModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(changePasswordModel, context, result, true);

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
        public void ChangePassword_with_AllEmpty()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = "",
                NewPassword = ""
            };

            // ACT
            var result = TryValidateChangePassword(changePasswordModel);

            // ASSERT
            Assert.Contains("OldPassword", result);
            Assert.Contains("NewPassword", result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void ChangePassword_with_AllValid()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = validOldPassword,
                NewPassword = validNewPassword
            };

            // ACT
            var result = TryValidateChangePassword(changePasswordModel);

            // ASSERT
            Assert.DoesNotContain("OldPassword", result);
            Assert.DoesNotContain("NewPassword", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void ChangePassword_with_OldEmpty()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = "",
                NewPassword = validNewPassword
            };

            // ACT
            var result = TryValidateChangePassword(changePasswordModel);

            // ASSERT
            Assert.Contains("OldPassword", result);
            Assert.DoesNotContain("NewPassword", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void ChangePassword_with_NewEmpty()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = validOldPassword,
                NewPassword = ""
            };

            // ACT
            var result = TryValidateChangePassword(changePasswordModel);

            // ASSERT
            Assert.DoesNotContain("OldPassword", result);
            Assert.Contains("NewPassword", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void ChangePassword_with_NewTooShort()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = validOldPassword,
                NewPassword = "Aa2!"
            };

            // ACT
            var result = TryValidateChangePassword(changePasswordModel);

            // ASSERT
            Assert.DoesNotContain("OldPassword", result);
            Assert.Contains("NewPassword", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void ChangePassword_with_NewMissingUppercase()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = validOldPassword,
                NewPassword = "password2!"
            };

            // ACT
            var result = TryValidateChangePassword(changePasswordModel);

            // ASSERT
            Assert.DoesNotContain("OldPassword", result);
            Assert.Contains("NewPassword", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void ChangePassword_with_NewMissingDigit()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = validOldPassword,
                NewPassword = "Password!"
            };

            // ACT
            var result = TryValidateChangePassword(changePasswordModel);

            // ASSERT
            Assert.DoesNotContain("OldPassword", result);
            Assert.Contains("NewPassword", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void ChangePassword_with_NewMissingSymbol()
        {
            // ARRANGE
            ChangePasswordModel changePasswordModel = new ChangePasswordModel
            {
                OldPassword = validOldPassword,
                NewPassword = "Password2"
            };

            // ACT
            var result = TryValidateChangePassword(changePasswordModel);

            // ASSERT
            Assert.DoesNotContain("OldPassword", result);
            Assert.Contains("NewPassword", result);
            Assert.Equal(1, result?.Count);
        }

    }

}