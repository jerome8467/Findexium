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
        private const string validRole = "Ok";

        private List<string> TryValidateSignUp(SignUpModel signUpModel)
        {
            var context = new ValidationContext(signUpModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(signUpModel, context, result, true);

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
        public void SignUpModel_With_AllEmpty()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = "",
                Password = "",
                Fullname = "",
                Role = ""
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.Contains("Password", result);
            Assert.Contains("Fullname", result);
            Assert.Contains("Role", result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void SignUpModel_With_AllValid()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = validFullname,
                Role = validRole
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void SignUpModel_With_UsernameEmpty()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = "",
                Password = validPassword,
                Fullname = validFullname,
                Role = validRole
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void SignUpModel_With_PasswordEmpty()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = validUsername,
                Password = "",
                Fullname = validFullname,
                Role = validRole
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void SignUpModel_With_PasswordTooShort()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = validUsername,
                Password = "Aa1!",
                Fullname = validFullname,
                Role = validRole
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void SignUpModel_With_PasswordMissingUppercase()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = validUsername,
                Password = "password1!",
                Fullname = validFullname,
                Role = validRole
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void SignUpModel_With_PasswordMissingDigit()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = validUsername,
                Password = "Password!",
                Fullname = validFullname,
                Role = validRole
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void SignUpModel_With_PasswordMissingSymbol()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = validUsername,
                Password = "Password1",
                Fullname = validFullname,
                Role = validRole
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void SignUpModel_With_FullNameEmpty()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = "",
                Role = validRole
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.Contains("Fullname", result);
            Assert.DoesNotContain("Role", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void SignUpModel_With_RolEmpty()
        {
            // ARRANGE
            SignUpModel signUpModel = new SignUpModel
            {
                Username = validUsername,
                Password = validPassword,
                Fullname = validFullname,
                Role = ""
            };

            // ACT
            var result = TryValidateSignUp(signUpModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.DoesNotContain("Fullname", result);
            Assert.Contains("Role", result);
            Assert.Equal(1, result?.Count);
        }

    }
}