using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // string configurationString = ConfigurationManager.ConnectionStrings["MsSqlServerConnectionString"].ConnectionString;

            optionsBuilder
                .UseSqlServer("Data Source=DESKTOP-UUC8F51;Server=localhost;Database=GamingPubsDatabase;Persist Security Info=True;User ID=stefan123;Password=123;TrustServerCertificate=True;TrustServerCertificate=True")
                .LogTo(Console.WriteLine);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<GamingPubGamingPlatform>()
            .HasKey(gp => new { gp.GamingPubId, gp.GamingPlatformId });
        }

        public DbSet<GamingPub> GamingPubs { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GamingPlatform> GamingPlatforms { get; set; }
        public DbSet<GamingPubGamingPlatform> GamingPubGamingPlatforms { get; set; }
    }
}