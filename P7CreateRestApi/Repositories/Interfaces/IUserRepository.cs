using P7CreateRestApi.Domain;

namespace P7CreateRestApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUser();
        public Task<User?> GetUserByUsername(string username);
        public Task<User?> GetUserById(int id);
        public Task AddUser(User user);
        public Task<bool> UpdateUser(User updatedUser);
        public Task<bool> ChangePassword(string newPassword, int id);
        public Task<bool> DeleteUser(int id);
    }
}
