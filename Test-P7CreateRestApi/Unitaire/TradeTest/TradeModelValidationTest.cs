using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Test_P7CreateRestApi.Unitaire.TradeTest
{
    public class TradeModelValidationTest
    {
        private const string validAccount = "Ok";
        private const string validAccountType = "Ok";
        private const string validTradeSecurity = "Ok";
        private const string validTradeStatus = "Ok";
        private const string validTrader = "Ok";
        private const string validBenchmark = "Ok";
        private const string validBook = "Ok";
        private const string validDealName = "Ok";
        private const string validDealType = "Ok";
        private const string validSourceListId = "Ok";
        private const string validSide = "Ok";

        private List<string> TryValidateTrade(TradeModel tradeModel)
        {
            var context = new ValidationContext(tradeModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(tradeModel, context, result, true);

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
        public void Trade_with_AllEmpty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = "",
                AccountType = "",
                TradeSecurity = "",
                TradeStatus = "",
                Trader = "",
                Benchmark = "",
                Book = "",
                DealName = "",
                DealType = "",
                SourceListId = "",
                Side = ""
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("Account", result);
            Assert.Contains("AccountType", result);
            Assert.Contains("TradeSecurity", result);
            Assert.Contains("TradeStatus", result);
            Assert.Contains("Trader", result);
            Assert.Contains("Benchmark", result);
            Assert.Contains("Book", result);
            Assert.Contains("DealName", result);
            Assert.Contains("DealType", result);
            Assert.Contains("SourceListId", result);
            Assert.Contains("Side", result);
            Assert.Equal(11, result?.Count);
        }

        [Fact]
        public void Trade_with_AllValid()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("AccountType", result);
            Assert.DoesNotContain("TradeSecurity", result);
            Assert.DoesNotContain("TradeStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void Trade_with_Account_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = "",
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("Account", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_AccountType_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = "",
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("AccountType", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_TradeSecurity_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = "",
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("TradeSecurity", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_TradeStatus_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = "",
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("TradeStatus", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_Trader_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = "",
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("Trader", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_Benchmark_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = "",
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("Benchmark", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_Book_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = "",
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("Book", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_DealName_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = "",
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("DealName", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_DealType_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = "",
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("DealType", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_SourceListId_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = "",
                Side = validSide
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("SourceListId", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void Trade_with_Side_Empty()
        {
            // ARRANGE
            TradeModel tradeModel = new TradeModel
            {
                Account = validAccount,
                AccountType = validAccountType,
                TradeSecurity = validTradeSecurity,
                TradeStatus = validTradeStatus,
                Trader = validTrader,
                Benchmark = validBenchmark,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = ""
            };

            // ACT
            var result = TryValidateTrade(tradeModel);

            // ASSERT
            Assert.Contains("Side", result);
            Assert.Equal(1, result?.Count);
        }
    }
}