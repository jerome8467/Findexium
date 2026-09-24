using P7CreateRestApi.Domain;


namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        public Task<IEnumerable<Rating>> GetAllRating();
        public Task<Rating?> GetRatingById(int id);
        public Task AddRating(Rating rating);
        public Task<bool> UpdateRating(Rating rating);
        public Task<bool> DeleteRating(int id);
    }
}
