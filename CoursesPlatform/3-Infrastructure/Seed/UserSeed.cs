using _2_Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3_Infrastructure.Seed;

public static class UserSeed
{
public static void Seed(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>().HasData(
        new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
        }
    );
}
}