using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models.UserModel;

namespace P7CreateRestApi.Data
{
    public static class IdentitySeedData
    {
        public static void EnsurePopulated(LocalDbContext context, IPasswordHasher<User> passwordHasher)
        {
            if (context.Users.Any())
                return;

            User admin = new User
            {
                Username = "Admin",
                Fullname = "Administrator",
                Role = nameof(UserRole.Admin)
            };
            admin.Password = passwordHasher.HashPassword(admin, "Password1!");

            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}