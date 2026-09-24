using System.Globalization;
using System.Resources;

namespace P7CreateRestApi.Ressource
{
    public class CurvePointModelRessources
    {
        private static readonly ResourceManager resourceManager = new ResourceManager(typeof(CurvePointModelRessources));
        private static readonly CultureInfo cultureInfo = new CultureInfo("fr-FR");

        public static string CurveIdRequired
        {
            get { return resourceManager.GetString("CurveIdRequired", cultureInfo) ?? string.Empty; }
        }

        public static string CurvePointNotFound
        {
            get { return resourceManager.GetString("CurvePointNotFound", cultureInfo) ?? string.Empty; }
        }

    }
}
