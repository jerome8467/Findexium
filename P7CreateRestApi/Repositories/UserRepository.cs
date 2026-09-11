using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository
    {
        private readonly LocalDbContext _DbContext;

        public UserRepository(LocalDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<User> FindByUserName(string userName)
        {
            return _DbContext.Users.Where(user => user.UserName == userName)
                                  .FirstOrDefault();
        }

        public async Task<List<User>> FindAll()
        {
            return await _DbContext.Users.ToListAsync();
        }

        public async Task Add(User user)
        {
            _DbContext.Users.Add(user);
            await _DbContext.SaveChangesAsync();
        }

        public async Task<User> FindById(int id)
        {
            return _DbContext.Users.Where(i => i.Id == id).FirstOrDefault();
            //return null;
        }
    }
}