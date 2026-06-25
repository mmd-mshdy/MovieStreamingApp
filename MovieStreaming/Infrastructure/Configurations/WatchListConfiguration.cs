using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Infrastructure.Configurations
{
    public class WatchListConfiguration : IEntityTypeConfiguration<WatchList>
    {
        public void Configure(EntityTypeBuilder<WatchList> builder)
        {
            builder.ToTable("WatchLists");

            builder.HasKey(w => w.Id);

            // Configure a composite unique index so a user cannot add the same movie twice
            builder.HasIndex(w => new { w.UserId, w.MovieId }).IsUnique();

            builder.Property(w => w.UserId)
                .IsRequired();

            builder.Property(w => w.MovieId)
                .IsRequired();

            builder.Property(w => w.AddedAt)
                .IsRequired();
        }
    }
}