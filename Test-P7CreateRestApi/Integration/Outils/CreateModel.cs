using P7CreateRestApi.Models;
using P7CreateRestApi.Models.UserModel;

namespace Test_P7CreateRestApi.Integration.Outils
{
    public static class  CreateModel
    {
        public static BidListModel NewValidBidListModel(string account)
        {
            return new BidListModel
            {
                Account = account,
                BidType = "Type1",
                Benchmark = "Benchmark1",
                Commentary = "Commentary1",
                BidSecurity = "Security1",
                BidStatus = "Status1",
                Trader = "Trader1",
                Book = "Book1",
                DealName = "Deal1",
                DealType = "DealType1",
                SourceListId = "Source1",
                Side = "Side1"
            };
        }

        public static CurvePointModel NewValidCurvePointModel(int curveId)
        {
            return new CurvePointModel
            {
                CurveId = curveId
            };
        }

        public static RatingModel NewValidRatingModel(string moodysRating, int orderNumber)
        {
            return new RatingModel
            {
                MoodysRating = moodysRating,
                SandPRating = "SandP1",
                FitchRating = "Fitch1",
                OrderNumber = orderNumber
            };
        }

        public static RuleNameModel NewValidRuleNameModel(string name)
        {
            return new RuleNameModel
            {
                Name = name,
                Description = "Description1",
                Json = "Json1",
                Template = "Template1",
                SqlStr = "SqlStr1",
                SqlPart = "SqlPart1"
            };
        }

        public static TradeModel NewValidTradeModel(string account)
        {
            return new TradeModel
            {
                Account = account,
                AccountType = "Test",
                TradeSecurity = "Test",
                TradeStatus = "Test",
                Trader = "Test",
                Benchmark = "Test",
                Book = "Test",
                DealName = "Test",
                DealType = "Test",
                SourceListId = "Test",
                Side = "Test"
            };
        }

        public static AdminSignUpModel NewValidAdminSignUpModel(string username, UserRole role)
        {
            return new AdminSignUpModel
            {
                Username = username,
                Password = "Password1!",
                Fullname = "Test",
                Role = role
            };
        }

        public static AdminUpdateModel NewValidAdminUpdateModel(string username, UserRole role)
        {
            return new AdminUpdateModel
            {
                Username = username,
                Fullname = "Test",
                Role = role
            };
        }

        public static UserSignUpModel NewValidUserSignUpModel(string username)
        {
            return new UserSignUpModel
            {
                Username = username,
                Password = "Password1!",
                Fullname = "Test"
            };
        }

        public static UserUpdateModel NewValidUserUpdateModel(string username)
        {
            return new UserUpdateModel
            {
                Username = username,
                Fullname = "Test"
            };
        }

        public static ChangePasswordModel NewValidChangePasswordModel(string oldPassword, string newPassword)
        {
            return new ChangePasswordModel
            {
                OldPassword = oldPassword,
                NewPassword = newPassword
            };
        }

        public static LoginModel NewValidLoginModel(string username, string password)
        {
            return new LoginModel
            {
                Username = username,
                Password = password
            };
        }
    }
}
