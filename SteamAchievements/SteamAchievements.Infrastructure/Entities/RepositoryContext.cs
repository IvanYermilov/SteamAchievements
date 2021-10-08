using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SteamAchievements.Infrastructure.Entities.Configuration;
using SteamAchievements.Infrastructure.Entities.Models;

namespace SteamAchievements.Infrastructure.Entities
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Developer> Developer { get; set; }
        public DbSet<Game> Games { get; set; }
    }

}
