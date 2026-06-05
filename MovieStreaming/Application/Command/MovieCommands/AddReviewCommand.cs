using MediatR;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Common.Result;
using MovieStreaming.Application.DTOs;

namespace MovieStreaming.Application.Command.MovieCommands
{
    public class AddReviewCommand(Guid Id, Guid movieId ,AddReviewDto dto) : IRequest<Result<Review>>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MovieId { get; } = movieId;
        public AddReviewDto Dto { get; } = dto;
    }
}