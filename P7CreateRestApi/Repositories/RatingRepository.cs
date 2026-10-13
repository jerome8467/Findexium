using Microsoft.EntityFrameworkCore;
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
            return await _DbContext.ratings.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddRating(Rating rating)
        {
            _DbContext.ratings.Add(rating);
            await _DbContext.SaveChangesAsync();
        }

        public async Task UpdateRating(Rating rating)
        {
            _DbContext.Update(rating);
            await _DbContext.SaveChangesAsync();
        }

        public async Task DeleteRating(Rating rating)
        {
            _DbContext.ratings.Remove(rating);
            await _DbContext.SaveChangesAsync();
        }
    }
}
