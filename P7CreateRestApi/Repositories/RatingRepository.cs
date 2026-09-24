using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Controllers.Domain;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories.Interfaces;

namespace P7CreateRestApi.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly LocalDbContext _DbContext;

        public RatingRepository(LocalDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<IEnumerable<Rating>> GetAllRating()
        {
            return await _DbContext.ratings.ToListAsync();
        }

        public async Task<Rating?> GetRatingById(int id)
        {
            return _DbContext.ratings.FirstOrDefault(r => r.Id == id);
        }

        public async Task AddRating(Rating rating)
        {
            _DbContext.ratings.Add(rating);
            await _DbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateRating(Rating rating)
        {
            Rating? findRating = await GetRatingById(rating.Id);
            if (findRating == null)
                return false;

            _DbContext.Entry(findRating).CurrentValues.SetValues(rating);
            await _DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRating(int id)
        {
            Rating? findRating = await GetRatingById(id);
            if (findRating == null)
                return false;

            _DbContext.ratings.Remove(findRating);
            await _DbContext.SaveChangesAsync();
            return true;
        }

    }
}
