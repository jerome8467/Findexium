using P7CreateRestApi.Models.UserModel;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.UserTest.Validation
{
    public class UpdateModelValidationTest
    {
        private const string validUsername = "Ok";
        private const string validFullname = "Ok";
        private const UserRole validRoleAdmin = UserRole.Admin;
        private const UserRole validRoleUser = UserRole.User;

        //////////////////////////// ADMIN ////////////////////////////

        private List<string> TryValidateUpdateAdmin(AdminUpdateModel adminUpdateModel)
        {
            var context = new ValidationContext(adminUpdateModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(adminUpdateModel, context, result, true);

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
        public void AdminUpdateModel_With_AllEmpty()
        {
            // ARRANGE
            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = "",
                Fullname = "",
                Role = (UserRole)0
            };

            // ACT
            var result = TryValidateUpdateAdmin(adminUpdateModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.Contains("Fullname", result);
            Assert.Contains("Role", result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void AdminUpdateModel_With_AllValid_RoleAdmin()
        {
            // ARRANGE
            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = validUsername,
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateUpdateAdmin(adminUpdateModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void AdminUpdateModel_With_AllValid_RoleUser()
        {
            // ARRANGE
            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = validUsername,
                Fullname = validFullname,
                Role = validRoleUser
            };

            // ACT
            var result = TryValidateUpdateAdmin(adminUpdateModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void AdminUpdateModel_With_UsernameEmpty()
        {
            // ARRANGE
            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = "",
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateUpdateAdmin(adminUpdateModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminUpdateModel_With_FullnameEmpty()
        {
            // ARRANGE
            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = validUsername,
                Fullname = "",
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateUpdateAdmin(adminUpdateModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminUpdateModel_With_RoleEmpty()
        {
            // ARRANGE
            AdminUpdateModel adminUpdateModel = new AdminUpdateModel
            {
                Username = validUsername,
                Fullname = validFullname,
                Role = (UserRole)0
            };

            // ACT
            var result = TryValidateUpdateAdmin(adminUpdateModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Contains("Role", result);
            Assert.Equal(1, result?.Count);
        }

        //////////////////////////// USER ////////////////////////////

        private List<string> TryValidateUpdateUser(UserUpdateModel userUpdateModel)
        {
            var context = new ValidationContext(userUpdateModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(userUpdateModel, context, result, true);

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
        public void UserUpdateModel_With_AllEmpty()
        {
            // ARRANGE
            UserUpdateModel userUpdateModel = new UserUpdateModel
            {
                Username = "",
                Fullname = ""
            };

            // ACT
            var result = TryValidateUpdateUser(userUpdateModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.Contains("Fullname", result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void UserUpdateModel_With_AllValid()
        {
            // ARRANGE
            UserUpdateModel userUpdateModel = new UserUpdateModel
            {
                Username = validUsername,
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateUpdateUser(userUpdateModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void UserUpdateModel_With_UsernameEmpty()
        {
            // ARRANGE
            UserUpdateModel userUpdateModel = new UserUpdateModel
            {
                Username = "",
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateUpdateUser(userUpdateModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UserUpdateModel_With_FullnameEmpty()
        {
            // ARRANGE
            UserUpdateModel userUpdateModel = new UserUpdateModel
            {
                Username = validUsername,
                Fullname = ""
            };

            // ACT
            var result = TryValidateUpdateUser(userUpdateModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

    }

}