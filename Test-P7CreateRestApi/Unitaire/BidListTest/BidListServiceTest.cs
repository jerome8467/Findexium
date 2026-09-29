using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;
using P7CreateRestApi.Repositories.Interfaces;
using P7CreateRestApi.Service;
using P7CreateRestApi.Service.Interfaces;
using System.Data;
using Xunit;


namespace Test_P7CreateRestApi.Unitaire.BidListTest
{
    public class BidListServiceTest : IDisposable
    {
        private readonly LocalDbContext _dbContext;
        private readonly IBidListRepository _bidListRepository;
        private readonly IBidListService _bidListService;

        public BidListServiceTest()
        {

            var options = new DbContextOptionsBuilder<LocalDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            _dbContext = new LocalDbContext(options);
            _dbContext.Database.OpenConnection();
            _dbContext.Database.EnsureCreated();

            _dbContext.BidLists.Add(CreateValidBidList(1, "Account1", "UserNameTempo1"));
            _dbContext.BidLists.Add(CreateValidBidList(2, "Account2", "UserNameTempo2"));
            _dbContext.BidLists.Add(CreateValidBidList(3, "Account3", "UserNameTempo3"));
            _dbContext.BidLists.Add(CreateValidBidList(4, "Account4", "UserNameTempo4"));
            _dbContext.SaveChanges();

            _bidListRepository = new BidListRepository(_dbContext);
            _bidListService = new BidListService(_bidListRepository);

        }

        private BidList CreateValidBidList(int id, string account, string usernameTempo)
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

        public void Dispose()
        {
            _dbContext.Database.CloseConnection();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task Service_GetAllBidList()
        {
            // ACT
            IEnumerable<BidList> bidLists = await _bidListService.GetAllBidList();

            //ASSERT
            Assert.Equal(4, bidLists.Count());
        }

        [Fact]
        public async Task Service_GetBidListById()
        {
            // ACT
            BidList? bidList = await _bidListService.GetBidListById(2);

            //ASSERT
            Assert.NotNull(bidList);
            Assert.Equal(2, bidList?.BidListId);
            Assert.Equal("Account2", bidList?.Account);
        }

        [Fact]
        public async Task Service_GetBidListModelById()
        {
            // ACT
            BidListModel? bidListModel = await _bidListService.GetBidListModelById(2);

            //ASSERT
            Assert.NotNull(bidListModel);
            Assert.Equal("Account2", bidListModel?.Account);
        }

        [Fact]
        public async Task Service_UpdateBidList()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = "Account1Update",
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
            string usernameIdentity = "JeromeAdmin";

            // ACT
            ServiceResult<BidList> serviceResult = await _bidListService.UpdateBidList(bidListModel, usernameIdentity, 1);
            BidList? bidList = await _dbContext.BidLists
                .AsNoTracking()
                .FirstAsync(i => i.BidListId == 1);

            // ASSERT
            Assert.NotNull(bidList);
            Assert.Equal("Account1Update", bidList?.Account);
            Assert.Equal("UserNameTempo1", bidList?.CreationName);
            Assert.Equal("JeromeAdmin", bidList?.RevisionName);
            Assert.NotNull(bidList?.RevisionDate);
            Assert.InRange(bidList!.RevisionDate!.Value, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.Empty(serviceResult?.Errors);
        }

        [Fact]
        public async Task Service_AddBidList()
        {
            // ARRANGE
            BidListModel bidListModel = new BidListModel
            {
                Account = "Account5",
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
            string usernameIdentity = "JeromeAdmin";

            // ACT
            ServiceResult<BidList> serviceResult = await _bidListService.AddBidList(bidListModel, usernameIdentity);

            BidList? bidList = await _dbContext.BidLists
                .AsNoTracking()
                .OrderBy(i => i.BidListId).LastAsync();
            
            int bidListsCount = await _dbContext.BidLists.CountAsync();

            // ASSERT
            Assert.Empty(serviceResult?.Errors);
            Assert.NotNull(bidList);
            Assert.NotNull(bidList?.RevisionDate);
            Assert.NotNull(bidList?.CreationDate);
            Assert.InRange(bidList!.RevisionDate!.Value, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.InRange(bidList!.CreationDate!.Value, DateTime.Now.AddMinutes(-1), DateTime.Now);
            Assert.Equal("Account5", bidList?.Account);
            Assert.Equal(5, bidList?.BidListId);
            Assert.Equal("JeromeAdmin", bidList?.CreationName);
            Assert.Equal("JeromeAdmin", bidList?.RevisionName);
            Assert.Equal(5, bidListsCount);
        }

        [Fact]
        public async Task Service_DeleteBidList()
        {
            // ACT
            await _bidListService.DeleteBidList(2);
            IEnumerable<BidList> bidLists = await _dbContext.BidLists
                .ToListAsync();

            // ASSERT
            Assert.Equal(3, bidLists.Count());
            Assert.Contains(bidLists, b => b.BidListId == 1);
            Assert.Contains(bidLists, b => b.BidListId == 3);
            Assert.Contains(bidLists, b => b.BidListId == 4);
            Assert.DoesNotContain(bidLists, b => b.BidListId == 2);
        }

    }
}
