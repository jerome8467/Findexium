using System.Globalization;
using System.Resources;

namespace P7CreateRestApi.Ressource
{
    public class RuleNameModelRessources
    {
        private static readonly ResourceManager resourceManager = new ResourceManager(typeof(RuleNameModelRessources));
        private static readonly CultureInfo cultureInfo = new CultureInfo("fr-FR");

        public static string NameRequired
        {
            get { return resourceManager.GetString("NameRequired", cultureInfo) ?? string.Empty; }
        }

        public static string DescriptionRequired
        {
            get { return resourceManager.GetString("DescriptionRequired", cultureInfo) ?? string.Empty; }
        }

        public static string JsonRequired
        {
            get { return resourceManager.GetString("JsonRequired", cultureInfo) ?? string.Empty; }
        }

        public static string TemplateRequired
        {
            get { return resourceManager.GetString("TemplateRequired", cultureInfo) ?? string.Empty; }
        }

        public static string SqlStrRequired
        {
            get { return resourceManager.GetString("SqlStrRequired", cultureInfo) ?? string.Empty; }
        }

        public static string SqlPartRequired
        {
            get { return resourceManager.GetString("SqlPartRequired", cultureInfo) ?? string.Empty; }
        }
        public static string RuleNameNotFound
        {
            get { return resourceManager.GetString("RuleNameNotFound", cultureInfo) ?? string.Empty; }
        }
    }
}
