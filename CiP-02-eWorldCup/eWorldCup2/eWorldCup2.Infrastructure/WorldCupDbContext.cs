using Microsoft.EntityFrameworkCore;
using eWorldCup2.Domain.Models;

namespace eWorldCup2.Infrastructure
{
    public class WorldCupDbContext : DbContext
    {
        public WorldCupDbContext(DbContextOptions options) : base(options) { }
  

        public DbSet<Player> Players { get; set; }

      //  protected override void OnModelCreating(ModelBuilder modelBuilder)
      //=> modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorldCupDbContext).Assembly);

    }
}
