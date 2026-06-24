using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Infrastructure.Repository
{
    public class WatchHistoryConfiguration
    : IEntityTypeConfiguration<WatchHistory>
    {
        public void Configure(EntityTypeBuilder<WatchHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.MovieId)
                .IsRequired();

            builder.Property(x => x.LastWatchedAt)
                .IsRequired();

            builder.HasIndex(x => new
            {
                x.UserId,
                x.MovieId
            })
            .IsUnique();
        }
    }
}
