using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.RuleNameTest
{
    public class RuleNameModelValidationTest
    {
        private const string validName = "Ok";
        private const string validDescription = "Ok";
        private const string validJson = "Ok";
        private const string validTemplate = "Ok";
        private const string validSqlStr = "Ok";
        private const string validSqlPart = "Ok";

        private List<string> TryValidateRuleName(RuleNameModel ruleNameModel)
        {

            var context = new ValidationContext(ruleNameModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(ruleNameModel, context, result, true);

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
        public void RuleName_with_AllEmpty()
        {
            // ARRANGE
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = "",
                Description = "",
                Json = "",
                Template = "",
                SqlStr = "",
                SqlPart = ""
            };

            // ACT
            var result = TryValidateRuleName(ruleNameModel);

            // ASSERT
            Assert.Contains("Name", result);
            Assert.Contains("Description", result);
            Assert.Contains("Json", result);
            Assert.Contains("Template", result);
            Assert.Contains("SqlStr", result);
            Assert.Contains("SqlPart", result);
            Assert.Equal(6, result?.Count);
        }

        [Fact]
        public void RuleName_with_AllValid()
        {
            // ARRANGE
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = validName,
                Description = validDescription,
                Json = validJson,
                Template = validTemplate,
                SqlStr = validSqlStr,
                SqlPart = validSqlPart
            };

            // ACT
            var result = TryValidateRuleName(ruleNameModel);

            // ASSERT
            Assert.DoesNotContain("Name", result);
            Assert.DoesNotContain("Description", result);
            Assert.DoesNotContain("Json", result);
            Assert.DoesNotContain("Template", result);
            Assert.DoesNotContain("SqlStr", result);
            Assert.DoesNotContain("SqlPart", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void RuleName_with_Name_Empty()
        {
            // ARRANGE
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = "",
                Description = validDescription,
                Json = validJson,
                Template = validTemplate,
                SqlStr = validSqlStr,
                SqlPart = validSqlPart
            };

            // ACT
            var result = TryValidateRuleName(ruleNameModel);

            // ASSERT
            Assert.Contains("Name", result);
            Assert.DoesNotContain("Description", result);
            Assert.DoesNotContain("Json", result);
            Assert.DoesNotContain("Template", result);
            Assert.DoesNotContain("SqlStr", result);
            Assert.DoesNotContain("SqlPart", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void RuleName_with_Description_Empty()
        {
            // ARRANGE
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = validName,
                Description = "",
                Json = validJson,
                Template = validTemplate,
                SqlStr = validSqlStr,
                SqlPart = validSqlPart
            };

            // ACT
            var result = TryValidateRuleName(ruleNameModel);

            // ASSERT
            Assert.DoesNotContain("Name", result);
            Assert.Contains("Description", result);
            Assert.DoesNotContain("Json", result);
            Assert.DoesNotContain("Template", result);
            Assert.DoesNotContain("SqlStr", result);
            Assert.DoesNotContain("SqlPart", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void RuleName_with_Json_Empty()
        {
            // ARRANGE
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = validName,
                Description = validDescription,
                Json = "",
                Template = validTemplate,
                SqlStr = validSqlStr,
                SqlPart = validSqlPart
            };

            // ACT
            var result = TryValidateRuleName(ruleNameModel);

            // ASSERT
            Assert.DoesNotContain("Name", result);
            Assert.DoesNotContain("Description", result);
            Assert.Contains("Json", result);
            Assert.DoesNotContain("Template", result);
            Assert.DoesNotContain("SqlStr", result);
            Assert.DoesNotContain("SqlPart", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void RuleName_with_Template_Empty()
        {
            // ARRANGE
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = validName,
                Description = validDescription,
                Json = validJson,
                Template = "",
                SqlStr = validSqlStr,
                SqlPart = validSqlPart
            };

            // ACT
            var result = TryValidateRuleName(ruleNameModel);

            // ASSERT
            Assert.DoesNotContain("Name", result);
            Assert.DoesNotContain("Description", result);
            Assert.DoesNotContain("Json", result);
            Assert.Contains("Template", result);
            Assert.DoesNotContain("SqlStr", result);
            Assert.DoesNotContain("SqlPart", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void RuleName_with_SqlStr_Empty()
        {
            // ARRANGE
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = validName,
                Description = validDescription,
                Json = validJson,
                Template = validTemplate,
                SqlStr = "",
                SqlPart = validSqlPart
            };

            // ACT
            var result = TryValidateRuleName(ruleNameModel);

            // ASSERT
            Assert.DoesNotContain("Name", result);
            Assert.DoesNotContain("Description", result);
            Assert.DoesNotContain("Json", result);
            Assert.DoesNotContain("Template", result);
            Assert.Contains("SqlStr", result);
            Assert.DoesNotContain("SqlPart", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void RuleName_with_SqlPart_Empty()
        {
            // ARRANGE
            RuleNameModel ruleNameModel = new RuleNameModel
            {
                Name = validName,
                Description = validDescription,
                Json = validJson,
                Template = validTemplate,
                SqlStr = validSqlStr,
                SqlPart = ""
            };

            // ACT
            var result = TryValidateRuleName(ruleNameModel);

            // ASSERT
            Assert.DoesNotContain("Name", result);
            Assert.DoesNotContain("Description", result);
            Assert.DoesNotContain("Json", result);
            Assert.DoesNotContain("Template", result);
            Assert.DoesNotContain("SqlStr", result);
            Assert.Contains("SqlPart", result);
            Assert.Equal(1, result?.Count);
        }

    }
}
