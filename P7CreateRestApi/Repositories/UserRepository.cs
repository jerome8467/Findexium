using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace P7CreateRestApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LocalDbContext _DbContext;

        public UserRepository(LocalDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            return await _DbContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByUsername(string userName)
        {
            return await _DbContext.Users.FirstOrDefaultAsync(user => user.Username == userName);
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _DbContext.Users.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddUser(User user)
        {
            _DbContext.Users.Add(user);
            await _DbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _DbContext.Update(user);
            await _DbContext.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            _DbContext.Users.Remove(user);
            await _DbContext.SaveChangesAsync();
        }


    }
}