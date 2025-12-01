using eWorldCup2.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace eWorldCup2.Infrastructure
{
    public class WorldCupDbContext : DbContext
    {
        public WorldCupDbContext(DbContextOptions options) : base(options) { }
  

        public DbSet<Player> Players { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Player>().HasData(
                new Player(1, "Alice"),
                new Player(2, "Bob"),
                new Player(3, "Charlie"),
                new Player(4, "Diana"),
                new Player(5, "Ethan"),
                new Player(6, "Fiona"),
                new Player(7, "George"),
                new Player(8, "Hannah"),
                new Player(9, "Isaac"),
                new Player(10, "Julia"),
                new Player(11, "Kevin"),
                new Player(12, "Laura"),
                new Player(13, "Michael"),
                new Player(14, "Nina"),
                new Player(15, "Oscar"),
                new Player(16, "Paula"),
                new Player(17, "Quentin"),
                new Player(18, "Rachel"),
                new Player(19, "Samuel"),
                new Player(20, "Tina")
            );
        }

        //  protected override void OnModelCreating(ModelBuilder modelBuilder)
        //=> modelBuilder.ApplyConfigurationsFromAssembly(typeof(WorldCupDbContext).Assembly);

    }
}
