using System;
using System.Collections.Generic;

namespace MovieStreaming.Application.DTOs
{
    // Added List<ReviewDto> reviews to the data transfer object contract
    public record MovieDto(
        Guid id,
        string title,
        string description,
        TimeSpan duration,
        DateOnly releaseDate,
        string videoUrl,
        string posterUrl,
        List<ReviewDto> reviews
    );
}