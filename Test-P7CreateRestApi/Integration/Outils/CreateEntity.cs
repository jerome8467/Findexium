using P7CreateRestApi.Domain;

namespace Test_P7CreateRestApi.Integration.Outils
{
    public static class CreateEntity
    {
        public static BidList NewValidBidList(int id, string account, string usernameTempo)
        {
            return new BidList
            {
                BidListId = id,
                Account = account,
                BidType = "Type1",
                Benchmark = "Benchmark1",
                Commentary = "Commentary1",
                BidSecurity = "Security1",
                BidStatus = "Status1",
                Trader = "Trader1",
                Book = "Book1",
                CreationName = usernameTempo,
                CreationDate = DateTime.Now,
                RevisionName = usernameTempo,
                RevisionDate = DateTime.Now,
                DealName = "Deal1",
                DealType = "DealType1",
                SourceListId = "Source1",
                Side = "Side1"
            };
        }

        public static CurvePoint NewValidCurvePoint(int id, byte curveId)
        {
            return new CurvePoint
            {
                Id = id,
                CurveId = curveId
            };
        }

        public static Rating NewValidRating(int id, string moodysRating, string SandPRating, string FitchRating, int OrderNumber)
        {
            return new Rating
            {
                Id = id,
                MoodysRating = moodysRating,
                SandPRating = SandPRating,
                FitchRating = FitchRating,
                OrderNumber = (byte)OrderNumber
            };
        }

        public static RuleName NewValidRuleName(int id, string Name, string Description, string Json, string Template, string SqlStr, string SqlPart)
        {
            return new RuleName
            {
                Id = id,
                Name = Name,
                Description = Description,
                Json = Json,
                Template = Template,
                SqlStr = SqlStr,
                SqlPart = SqlPart
            };
        }

        public static Trade NewValidTrade(int id, string Account, string CreationName)
        {
            return new Trade
            {
                TradeId = id,
                Account = Account,
                CreationName = CreationName,
                CreationDate = DateTime.Now,
                RevisionName = CreationName,
                RevisionDate = DateTime.Now,
                AccountType = "Test",
                TradeSecurity = "Test",
                TradeStatus = "Test",
                Trader = "Test",
                Benchmark = "Test",
                Book = "Test",
                DealName = "Test",
                DealType = "Test",
                SourceListId = "Test",
                Side = "Test",
            };
        }
    }
}
