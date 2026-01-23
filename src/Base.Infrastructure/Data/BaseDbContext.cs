using Base.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Infrastructure.Data
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.UserName).IsUnique();
                b.HasIndex(u => u.Email).IsUnique();

                b.HasMany(u => u.Roles)
                 .WithMany(r => r.Users)
                 .UsingEntity(j => j.ToTable("UserRoles"));
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.Name).IsUnique();
            });
        }

    }
}
