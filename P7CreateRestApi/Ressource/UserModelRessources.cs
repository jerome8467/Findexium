using System.Globalization;
using System.Resources;

namespace P7CreateRestApi.Ressource
{
    public class UserModelRessources
    {

        private static readonly ResourceManager resourceManager = new ResourceManager(typeof(UserModelRessources));
        private static readonly CultureInfo cultureInfo = new CultureInfo("fr-FR");

        public static string UsernameRequired
        {
            get{ return resourceManager.GetString("UsernameRequired", cultureInfo) ?? string.Empty;}
        }

        public static string PasswordRequired
        {
            get{ return resourceManager.GetString("PasswordRequired", cultureInfo) ?? string.Empty;}
        } 

        public static string FullnameRequired
        {
            get{ return resourceManager.GetString("FullnameRequired", cultureInfo) ?? string.Empty;}
        }

        public static string RoleRequired
        {
            get{ return resourceManager.GetString("RoleRequired", cultureInfo) ?? string.Empty;}
        }

        public static string OldPasswordRequired
        {
            get{ return resourceManager.GetString("OldPasswordRequired", cultureInfo) ?? string.Empty;}
        }

        public static string NewPasswordRequired
        {
            get{ return resourceManager.GetString("NewPasswordRequired", cultureInfo) ?? string.Empty;}
        }

        public static string PasswordTooShort
        {
            get{ return resourceManager.GetString("PasswordTooShort", cultureInfo) ?? string.Empty;}
        }

        public static string PasswordMissingUppercase
        {
            get{ return resourceManager.GetString("PasswordMissingUppercase", cultureInfo) ?? string.Empty;}
        }

        public static string PasswordMissingDigit
        {
            get{ return resourceManager.GetString("PasswordMissingDigit", cultureInfo) ?? string.Empty;}
        }

        public static string PasswordMissingSymbol
        {
            get{ return resourceManager.GetString("PasswordMissingSymbol", cultureInfo) ?? string.Empty;}
        }

        public static string UsernameUnavailable
        {
            get{ return resourceManager.GetString("UsernameUnavailable", cultureInfo) ?? string.Empty;}
        }

        public static string UserNotFound
        {
            get{ return resourceManager.GetString("UserNotFound", cultureInfo) ?? string.Empty;}
        }

        public static string OldPasswordIncorrect
        {
            get{ return resourceManager.GetString("OldPasswordIncorrect", cultureInfo) ?? string.Empty;}
        }

    }
}
