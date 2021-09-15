using Entities.Configuration;
using SteamAchievements.InfoStructure.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SteamAchievements.InfoStructure.Entities
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

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
