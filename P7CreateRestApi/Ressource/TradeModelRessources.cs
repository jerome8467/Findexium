using System.Globalization;
using System.Resources;

namespace P7CreateRestApi.Ressource
{
    public class TradeModelRessources
    {

        private static readonly ResourceManager resourceManager = new ResourceManager(typeof(TradeModelRessources));
        private static readonly CultureInfo cultureInfo = new CultureInfo("fr-FR");

        public static string AccountRequired
        {
            get { return resourceManager.GetString("AccountRequired", cultureInfo) ?? string.Empty;}
        }

        public static string AccountTypeRequired
        {
            get { return resourceManager.GetString("AccountTypeRequired", cultureInfo) ?? string.Empty; }
        }

        public static string TradeSecurityRequired
        {
            get { return resourceManager.GetString("TradeSecurityRequired", cultureInfo) ?? string.Empty; }
        }

        public static string TradeStatusRequired
        {
            get { return resourceManager.GetString("TradeStatusRequired", cultureInfo) ?? string.Empty; }
        }

        public static string TraderRequired
        {
            get { return resourceManager.GetString("TraderRequired", cultureInfo) ?? string.Empty; }
        }

        public static string BenchmarkRequired
        {
            get { return resourceManager.GetString("BenchmarkRequired", cultureInfo) ?? string.Empty; }
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
        public static string TradeNotFound
        {
            get { return resourceManager.GetString("TradeNotFound", cultureInfo) ?? string.Empty; }
        }

    }
}
