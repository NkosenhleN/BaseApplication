using Base.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Data.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(BaseDbContext context)
        {
            if (await context.Roles.AnyAsync())
                return; 

            var roles = new List<Role>
        {
            new Role("Admin"),
            new Role("User")
        };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }
    }
}
