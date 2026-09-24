using System.Globalization;
using System.Resources;

namespace P7CreateRestApi.Ressource
{
    public class RatingModelRessources
    {
        public static readonly ResourceManager resourceManager = new ResourceManager(typeof(RatingModelRessources));
        public static readonly CultureInfo cultureInfo = new CultureInfo("fr-FR");

        public static string MoodysRatingRequired
        {
            get { return resourceManager.GetString("MoodysRatingRequired", cultureInfo) ?? string.Empty; }
        }

        public static string SandPRatingRequired
        {
            get { return resourceManager.GetString("SandPRatingRequired", cultureInfo) ?? string.Empty; }
        }

        public static string FitchRatingRequired
        {
            get { return resourceManager.GetString("FitchRatingRequired", cultureInfo) ?? string.Empty; }
        }
        public static string RatingNotFound
        {
            get { return resourceManager.GetString("RatingNotFound", cultureInfo) ?? string.Empty; }
        }

    }
}
