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

        public async Task<bool> UpdateUser(User user)
        {
            User? findUser = await GetUserById(user.Id);
            if (findUser == null)
                return false;

            findUser.Username = user.Username;
            findUser.Fullname = user.Fullname;
            findUser.Role = user.Role;
            await _DbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> ChangePassword(string newPassword, int id)
        {
            User? findUser = await GetUserById(id);
            if (findUser == null)
                return false;

            findUser.Password = newPassword;
            await _DbContext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteUser(int id)
        {
            User? deletedUser = await GetUserById(id);
            if (deletedUser == null) 
                return false;

            _DbContext.Users.Remove(deletedUser);
            await _DbContext.SaveChangesAsync();
                return true;
        }


    }
}