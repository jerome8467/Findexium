using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain;

namespace Test_P7CreateRestApi.Integration.Outils
{
    public static class CreateUser
    {
        private static readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public static User NewValidUser(int id, string username, string role)
        {
            User user = new User
            {
                Id = id,
                Username = username,
                Fullname = "test",
                Role = role
            };
            user.Password = _passwordHasher.HashPassword(user, "Password1!");

            return user;
        }
    }
}