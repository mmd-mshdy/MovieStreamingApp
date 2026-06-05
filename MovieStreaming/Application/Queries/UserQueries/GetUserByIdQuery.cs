using MediatR;
using MovieStreaming.Domain.Aggregates.Users;
using MovieStreaming.Domain.Common.Result;
namespace MovieStreaming.Application.Queries.UserQueries
{
    public record GetUserByIdQuery(Guid id) : IRequest<Result<User>>;
}
