using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Persistence
{
    public static class Seed
    {
        public static void SeedDataUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = 1,
                    Name = "Administrator",
                    Email = "admin@roomservice.com",
                    PhoneNumber = "08123456789",
                    Password = "$2a$11$psFE6UAQOjSryTMxQyNq2.ltx3tFRVOCeDE0/8AzstYGGERQWQ2lS",
                    Role = UserRole.Admin,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = (DateTime?)null
                }
            );
        }
    }
}