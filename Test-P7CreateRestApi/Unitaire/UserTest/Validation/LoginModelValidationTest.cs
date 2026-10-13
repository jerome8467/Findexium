using P7CreateRestApi.Models.UserModel;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.UserTest.Validation
{
    public class LoginModelValidationTest
    {
        private const string validUsername = "Ok";
        private const string validPassword = "Password@1";

        private List<string> TryValidateLogin(LoginModel loginModel)
        {

            var context = new ValidationContext(loginModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(loginModel, context, result, true);

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
        public void Login_with_AllEmpty()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = "",
                Password = ""
            };

            // ACT
            var result = TryValidateLogin(loginModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.Contains("Password", result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Login_with_AllValid()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = validUsername,
                Password = validPassword
            };

            // ACT
            var result = TryValidateLogin(loginModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void Login_with_Password_Empty()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = validUsername,
                Password = ""
            };

            // ACT
            var result = TryValidateLogin(loginModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Login_with_PasswordTooShort()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = validUsername,
                Password = "Aa1!"
            };

            // ACT
            var result = TryValidateLogin(loginModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Login_with_PasswordMissingUppercase()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = validUsername,
                Password = "password1!"
            };

            // ACT
            var result = TryValidateLogin(loginModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Login_with_PasswordMissingDigit()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = validUsername,
                Password = "Password!"
            };

            // ACT
            var result = TryValidateLogin(loginModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Login_with_PasswordMissingSymbol()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = validUsername,
                Password = "Password1"
            };

            // ACT
            var result = TryValidateLogin(loginModel);

            // ASSERT
            Assert.DoesNotContain("Username", result);
            Assert.Contains("Password", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Login_with_UsernameEmpty()
        {
            // ARRANGE
            LoginModel loginModel = new LoginModel
            {
                Username = "",
                Password = validPassword
            };

            // ACT
            var result = TryValidateLogin(loginModel);

            // ASSERT
            Assert.Contains("Username", result);
            Assert.DoesNotContain("Password", result);
            Assert.Equal(1, result?.Count);
        }
    }
}
