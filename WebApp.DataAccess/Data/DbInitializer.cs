using Microsoft.Extensions.DependencyInjection;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Enums;

namespace WebApp.DataAccess.Data;

public static class DbInitializer
{
    public static void SeedAdminUser(IServiceProvider serviceProvider)
    {
        using (var context = serviceProvider.GetRequiredService<ForumDbContext>())
        {
            // Check if any users exist, and if there's no admin, add one
            if (!context.Users.Any(u => u.Role == UserRole.Admin))
            {
                var adminUser = new User
                {
                    Nickname = "admin",
                    Email = "admin@example.com",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    PasswordHash = "$2a$11$tWIIJpYJjZWMRXnYn0CNqeAxMBCEttbT2.5UAtnEVnsLLJj223Kte",
                };

                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
}
