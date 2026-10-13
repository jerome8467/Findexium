using P7CreateRestApi.Models.UserModel;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.UserTest.Validation
{
    public class SignUpModelValidationTest
    {
        private const string validUsername = "Ok";
        private const string validPassword = "Password@1";
        private const string validFullname = "Ok";
        private const UserRole validRoleAdmin = UserRole.Admin;
        private const UserRole validRoleUser = UserRole.User;

        //////////////////////////// ADMIN ////////////////////////////

        private List<string> TryValidateSignUpAdmin(AdminSignUpModel adminSignUpModel)
        {
            var context = new ValidationContext(adminSignUpModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(adminSignUpModel, context, result, true);

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
        public void AdminSignUpModel_With_AllEmpty()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = "",
                Password = "",
                Fullname = "",
                Role = (UserRole)0
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.Contains("Password", result);
            Assert.Contains("Fullname", result);
            Assert.Contains("Role", result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_AllValid_RoleAdmin()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_AllValid_RoleUser()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = validFullname,
                Role = validRoleUser
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_UsernameEmpty()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = "",
                Password = validPassword,
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_PasswordEmpty()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = "",
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_PasswordTooShort()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = "Aa1!",
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_PasswordMissingUppercase()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = "password1!",
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_PasswordMissingDigit()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = "Password!",
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_PasswordMissingSymbol()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = "Password1",
                Fullname = validFullname,
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_FullnameEmpty()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = "",
                Role = validRoleAdmin
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.Contains("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void AdminSignUpModel_With_RoleEmpty()
        {
            // ARRANGE
            AdminSignUpModel adminSignUpModel = new AdminSignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = validFullname,
                Role = (UserRole)0
            };

            // ACT
            var result = TryValidateSignUpAdmin(adminSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Contains("Role", result);
            Assert.Equal(1, result?.Count);
        }

        //////////////////////////// USER ////////////////////////////

        private List<string> TryValidateSignUpUser(UserSignUpModel userSignUpModel)
        {
            var context = new ValidationContext(userSignUpModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(userSignUpModel, context, result, true);

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
        public void UserSignUpModel_With_AllEmpty()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = "",
                Password = "",
                Fullname = ""
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.Contains("Password", result);
            Assert.Contains("Fullname", result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void UserSignUpModel_With_AllValid()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void UserSignUpModel_With_UsernameEmpty()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = "",
                Password = validPassword,
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UserSignUpModel_With_PasswordEmpty()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = validUsername,
                Password = "",
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UserSignUpModel_With_PasswordTooShort()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = validUsername,
                Password = "Aa1!",
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UserSignUpModel_With_PasswordMissingUppercase()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = validUsername,
                Password = "password1!",
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UserSignUpModel_With_PasswordMissingDigit()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = validUsername,
                Password = "Password!",
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UserSignUpModel_With_PasswordMissingSymbol()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = validUsername,
                Password = "Password1",
                Fullname = validFullname
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void UserSignUpModel_With_FullnameEmpty()
        {
            // ARRANGE
            UserSignUpModel userSignUpModel = new UserSignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = ""
            };

            // ACT
            var result = TryValidateSignUpUser(userSignUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.Contains("Fullname", result);
            Assert.Equal(1, result?.Count);
        }

    }
}