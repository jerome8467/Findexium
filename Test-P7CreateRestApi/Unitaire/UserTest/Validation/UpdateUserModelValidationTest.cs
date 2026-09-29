using P7CreateRestApi.Models.UserModel;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.UserTest.Validation
{
    public class UpdateUserModelValidationTest
    {
        private const string validUsername = "Ok";
        private const string validFullName = "Ok";
        private const string validRole = "Ok";

        private List<String> TryValidateUpdateUser(UpdateUserModel updateUserModel)
        {
            var context = new ValidationContext(updateUserModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(updateUserModel, context, result, true);

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
        public void UpdateUserModel_With_AllEmpty()
        {
            // ARRANGE
            UpdateUserModel updateUserModel = new UpdateUserModel
            {
                Username = "",
                Fullname = "",
                Role = "",
            };

            // ACT
            var result = TryValidateUpdateUser(updateUserModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.Contains("Fullname", result);
            Assert.Contains("Role", result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void UpdateUserModel_With_AllValid()
        {
            // ARRANGE
            UpdateUserModel updateUserModel = new UpdateUserModel
            {
                Username = validUsername,
                Fullname = validFullName,
                Role = validRole,
            };

            // ACT
            var result = TryValidateUpdateUser(updateUserModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void UpdateUserModel_With_UsernameEmpty()
        {
            // ARRANGE
            UpdateUserModel updateUserModel = new UpdateUserModel
            {
                Username = "",
                Fullname = validFullName,
                Role = validRole,
            };

            // ACT
            var result = TryValidateUpdateUser(updateUserModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UpdateUserModel_With_FullnameEmpty()
        {
            // ARRANGE
            UpdateUserModel updateUserModel = new UpdateUserModel
            {
                Username = validUsername,
                Fullname = "",
                Role = validRole,
            };

            // ACT
            var result = TryValidateUpdateUser(updateUserModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UpdateUserModel_With_RoleEmpty()
        {
            // ARRANGE
            UpdateUserModel updateUserModel = new UpdateUserModel
            {
                Username = validUsername,
                Fullname = validFullName,
                Role = "",
            };

            // ACT
            var result = TryValidateUpdateUser(updateUserModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Contains("Role", result);
            Assert.Equal(1, result?.Count);
        }

    }

}
