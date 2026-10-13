using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;


namespace Test_P7CreateRestApi.Unitaire.BidListTest
{
    public class BidListModelValidationTests
    {
        private const string validAccount = "Ok";
        private const string validBidType = "Ok";
        private const string validBenchmark = "Ok";
        private const string validCommentary = "Ok";
        private const string validBidSecurity = "Ok";
        private const string validBidStatus = "Ok";
        private const string validTrader = "Ok";
        private const string validBook = "Ok";
        private const string validDealName = "Ok";
        private const string validDealType = "Ok";
        private const string validSourceListId = "Ok";
        private const string validSide = "Ok";

        private List<string> TryValidateBidList(BidListModel bidListModel)
        {

            var context = new ValidationContext(bidListModel);
            var result = new List<ValidationResult>();
            Validator.TryValidateObject(bidListModel, context, result, true);

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
        public void BidList_with_AllEmpty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = "",
                BidType = "",
                Benchmark = "",
                Commentary = "",
                BidSecurity = "",
                BidStatus = "",
                Trader = "",
                Book = "",
                DealName = "",
                DealType = "",
                SourceListId = "",
                Side = ""
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.Contains("Account", result);
            Assert.Contains("BidType", result);
            Assert.Contains("Benchmark", result);
            Assert.Contains("Commentary", result);
            Assert.Contains("BidSecurity", result);
            Assert.Contains("BidStatus", result);
            Assert.Contains("Trader", result);
            Assert.Contains("Book", result);
            Assert.Contains("DealName", result);
            Assert.Contains("DealType", result);
            Assert.Contains("SourceListId", result);
            Assert.Contains("Side", result);
            Assert.Equal(12, result.Count);
        }

        [Fact]
        public void BidList_with_AllValid()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(0, result?.Count);
        }

        [Fact]
        public void BidList_with_Account_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = "",
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.Contains("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_BidType_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = "",
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.Contains("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_Benchmark_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = "",
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.Contains("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_Commentary_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = "",
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.Contains("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_BidSecurity_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = "",
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.Contains("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_BidStatus_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = "",
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.Contains("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_Trader_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = "",
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.Contains("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_Book_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = "",
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.Contains("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_DealName_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = "",
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.Contains("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_DealType_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = "",
                SourceListId = validSourceListId,
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.Contains("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_SourceListId_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = "",
                Side = validSide
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.Contains("SourceListId", result);
            Assert.DoesNotContain("Side", result);
            Assert.Equal(1, result?.Count);
        }

        [Fact]
        public void BidList_with_Side_Empty()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = validAccount,
                BidType = validBidType,
                Benchmark = validBenchmark,
                Commentary = validCommentary,
                BidSecurity = validBidSecurity,
                BidStatus = validBidStatus,
                Trader = validTrader,
                Book = validBook,
                DealName = validDealName,
                DealType = validDealType,
                SourceListId = validSourceListId,
                Side = ""
            };

            // ACT
            var result = TryValidateBidList(bidListModel);

            // ASSERT
            Assert.DoesNotContain("Account", result);
            Assert.DoesNotContain("BidType", result);
            Assert.DoesNotContain("Benchmark", result);
            Assert.DoesNotContain("Commentary", result);
            Assert.DoesNotContain("BidSecurity", result);
            Assert.DoesNotContain("BidStatus", result);
            Assert.DoesNotContain("Trader", result);
            Assert.DoesNotContain("Book", result);
            Assert.DoesNotContain("DealName", result);
            Assert.DoesNotContain("DealType", result);
            Assert.DoesNotContain("SourceListId", result);
            Assert.Contains("Side", result);
            Assert.Equal(1, result?.Count);
        }
    }
}
