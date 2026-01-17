using Base.Domain.Entities;
using Base.Infrastructure.Data;
using Base.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Tests.Helpers
{
    public static class TestDbContextFactory
    {
        public static BaseDbContext Create(bool seedData = true)
        {
            var options = new DbContextOptionsBuilder<BaseDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new BaseDbContext(options);

            if (seedData)
                SeedUsers(context);

            return context;
        }

        private static void SeedUsers(BaseDbContext context)
        {
            var hasher = new PasswordHasher();
            var (hash, salt) = hasher.HashPassword("Admin123!");

            context.Users.Add(new User("admin", "System", "Admin", "admin@example.com", hash, salt));
            context.Users.Add(new User("user1", "John", "Doe", "john@example.com", hash, salt));

            context.SaveChanges();
        }
    }
}
