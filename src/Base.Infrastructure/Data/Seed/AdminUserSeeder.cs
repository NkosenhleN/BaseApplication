using Base.Application.Interfaces;
using Base.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Data.Seed
{
    public static class AdminUserSeeder
    {
        public static async Task SeedAsync(
        BaseDbContext context,
        IPasswordHasher passwordHasher)
        {
            if (await context.Users.AnyAsync(u => u.UserName == "admin"))
                return;

            var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");

            var (hash, salt) = passwordHasher.HashPassword("Admin@123");

            var admin = new User(
                username: "admin",
                firstName: "System",
                lastName: "Administrator",
                email: "admin@system.local",
                passwordHash: hash,
                passwordSalt: salt
            );

            admin.AssignRole(adminRole);

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}
