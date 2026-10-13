using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using P7CreateRestApi.Ressource;


namespace P7CreateRestApi.Attribute
{
    public class PasswordValidAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? password = value as string;
            string[] memberNames = new[] { validationContext.MemberName ?? string.Empty };

            if (string.IsNullOrEmpty(password))
                return new ValidationResult(UserModelRessources.PasswordRequired,memberNames);

            if (password.Length < 8)
                return new ValidationResult(UserModelRessources.PasswordTooShort, memberNames);

            if (!Regex.IsMatch(password, @"[A-Z]"))
                return new ValidationResult(UserModelRessources.PasswordMissingUppercase, memberNames);

            if (!Regex.IsMatch(password, @"\d"))
                return new ValidationResult(UserModelRessources.PasswordMissingDigit, memberNames);

            if (!Regex.IsMatch(password, @"[^a-zA-Z\d]"))
                return new ValidationResult(UserModelRessources.PasswordMissingSymbol, memberNames);
            return ValidationResult.Success;

        }

    }
}
