using MediatR;
using MovieStreaming.Application.DTOs;

namespace MovieStreaming.Application.Queries.WatchListQueries
{
    public record GetWatchListQuery : IRequest<List<WatchListDto>>;


}