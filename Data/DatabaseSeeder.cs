using BCrypt.Net;
using IdeaPulse.Models;

namespace IdeaPulse.Data;

public class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        // Ensure admin user exists
        if (!context.Users.Any(u => u.Email == "admin@ideapulse.com"))
        {
            var adminPassword = BCrypt.Net.BCrypt.HashPassword("admin123");
            
            var admin = new User
            {
                Name = "Admin User",
                Email = "admin@ideapulse.com",
                PasswordHash = adminPassword,
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            context.SaveChanges();
        }

        // Create a demo user
        if (!context.Users.Any(u => u.Email == "demo@ideapulse.com"))
        {
            var demoPassword = BCrypt.Net.BCrypt.HashPassword("demo123");
            
            var demo = new User
            {
                Name = "Demo User",
                Email = "demo@ideapulse.com",
                PasswordHash = demoPassword,
                Role = "Entrepreneur",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(demo);
            context.SaveChanges();
        }
    }
}

