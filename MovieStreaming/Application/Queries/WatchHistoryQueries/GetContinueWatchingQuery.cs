using MediatR;
public record GetContinueWatchingQuery
    : IRequest<List<ContinueWatchingDto>>;

