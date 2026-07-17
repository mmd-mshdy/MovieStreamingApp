using Microsoft.EntityFrameworkCore;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Aggregates.Users;
namespace MovieStreaming.Infrastructure
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Movie> Movies { get; set; } 
        public DbSet<Review> Reviews { get; set; } 
        public DbSet<User> Users { get; set; }
        public DbSet<WatchHistory> WatchHistories { get; set; }
        public DbSet<WatchList> WatchLists{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
                .HasMany(movie => movie.Genres)
                .WithMany()
                .UsingEntity(join => join.ToTable("MovieGenres"));

            modelBuilder.Entity<Movie>()
                .Navigation(movie => movie.Genres)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }

    }

