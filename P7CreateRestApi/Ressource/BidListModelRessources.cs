using System.Globalization;
using System.Resources;

namespace P7CreateRestApi.Ressource
{
    public class BidListModelRessources
    {
        private static readonly ResourceManager resourceManager = new ResourceManager(typeof(BidListModelRessources));
        private static readonly CultureInfo cultureInfo = new CultureInfo("fr-FR");

        public static string AccountRequired
        {
            get { return resourceManager.GetString("AccountRequired", cultureInfo) ?? string.Empty; }
        }

        public static string BidTypeRequired
        {
            get { return resourceManager.GetString("BidTypeRequired", cultureInfo) ?? string.Empty; }
        }

        public static string BenchmarkRequired
        {
            get { return resourceManager.GetString("BenchmarkRequired", cultureInfo) ?? string.Empty; }
        }

        public static string CommentaryRequired
        {
            get { return resourceManager.GetString("CommentaryRequired", cultureInfo) ?? string.Empty; }
        }

        public static string BidSecurityRequired
        {
            get { return resourceManager.GetString("BidSecurityRequired", cultureInfo) ?? string.Empty; }
        }

        public static string BidStatusRequired
        {
            get { return resourceManager.GetString("BidStatusRequired", cultureInfo) ?? string.Empty; }
        }

        public static string TraderRequired
        {
            get { return resourceManager.GetString("TraderRequired", cultureInfo) ?? string.Empty; }
        }

        public static string BookRequired
        {
            get { return resourceManager.GetString("BookRequired", cultureInfo) ?? string.Empty; }
        }

        public static string DealNameRequired
        {
            get { return resourceManager.GetString("DealNameRequired", cultureInfo) ?? string.Empty; }
        }

        public static string DealTypeRequired
        {
            get { return resourceManager.GetString("DealTypeRequired", cultureInfo) ?? string.Empty; }
        }

        public static string SourceListIdRequired
        {
            get { return resourceManager.GetString("SourceListIdRequired", cultureInfo) ?? string.Empty; }
        }

        public static string SideRequired
        {
            get { return resourceManager.GetString("SideRequired", cultureInfo) ?? string.Empty; }
        }

        public static string BidListNotFound
        {
            get { return resourceManager.GetString("BidListNotFound", cultureInfo) ?? string.Empty; }
        }


    }
}
