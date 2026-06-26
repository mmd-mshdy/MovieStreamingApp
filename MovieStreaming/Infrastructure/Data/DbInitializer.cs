// Location: MovieStreaming/Infrastructure/Data/DbInitializer.cs
using System.Text.Json;
using MovieStreaming.Domain.Aggregates.Movies;
using System.Globalization;

namespace MovieStreaming.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            Console.WriteLine("--> Checking Database Seeding...");
            // 1. Ensure the database is fully created and migrated
            await context.Database.EnsureCreatedAsync();

            // 2. If movies already exist, don't double-seed
            if (context.Set<Movie>().Any())
            {
                Console.WriteLine("--> Movies already exist. Skipping seed.");
                return;
            }

            var filePath = Path.Combine(AppContext.BaseDirectory, "movies-seed.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"--> ERROR: Cannot find seed file at: {filePath}");
                return;
            }
            Console.WriteLine("--> Found seed file! Deserializing...");

            // 3. Read and Deserialize the data
            var jsonString = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // This is where serialization/deserialization magic happens!
            var omdbMovies = JsonSerializer.Deserialize<List<ExtendedOmdbModel>>(jsonString, options);

            if (omdbMovies == null) return;

            foreach (var item in omdbMovies)
            {
                // Parse OMDb's "148 min" format safely to a C# TimeSpan
                int minutes = 120; // Default fallback
                if (!string.IsNullOrEmpty(item.Runtime) && item.Runtime.Contains(" min"))
                {
                    int.TryParse(item.Runtime.Replace(" min", "").Trim(), out minutes);
                }
                var duration = TimeSpan.FromMinutes(minutes);

                // Parse OMDb's "16 Jul 2010" release date format safely to DateOnly
                var releaseDate = DateOnly.FromDateTime(DateTime.UtcNow); // Default fallback
                if (DateOnly.TryParseExact(item.Released, "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                {
                    releaseDate = parsedDate;
                }

                // 4. Map to your exact domain entity definition
                var movie = new Movie(
                    Guid.NewGuid(),
                    item.Title,
                    item.Plot,
                    duration,
                    releaseDate
                );

                // Append your poster and custom streaming stream links
                movie.UpdateMediaUrls(item.Poster, item.VideoUrl);

                context.Set<Movie>().Add(movie);
            }

            // 5. Commit everything to SQL Server safely
            await context.SaveChangesAsync();
            Console.WriteLine("--> Database seeded successfully!");
        }
    }

    // Temporary class extending the base OMDB response to include our custom Video URL asset
    public class ExtendedOmdbModel : OmdbSeedModel
    {
        public string VideoUrl { get; set; } = string.Empty;
    }
}