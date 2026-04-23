using Microsoft.EntityFrameworkCore;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Domain.ValueObjects;
namespace MovieStreaming.Infrastructure
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Movie> Movies { get; set; } 
        public DbSet<Review> Reviews { get; set; } 
        public DbSet<User> Users { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<CastMembers> CastMembers { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Reviews)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId);

            modelBuilder.Entity<User>().HasMany(u => u.WatchLists);
            modelBuilder.Entity<User>().OwnsOne(u => u.WalletBallance, moneyBuilder =>
            {
                moneyBuilder.Property(m => m.Amount).HasColumnName("WalletAmount");
                moneyBuilder.Property(m => m.Amount).HasPrecision(18, 2);
                moneyBuilder.Property(m => m.Currency).HasColumnName("WalletCurrency");
            }
            );
        }

    }
}
