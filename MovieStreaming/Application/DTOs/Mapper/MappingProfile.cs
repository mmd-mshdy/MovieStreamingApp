using AutoMapper;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Aggregates.Users;

namespace MovieStreaming.Application.DTOs.Mapper;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateMovieDto, Movie>();
        CreateMap<Movie, CreateMovieDto>();

        CreateMap<Review, ReviewDto>()
            .ForCtorParam(
                "userName",
                options => options.MapFrom(
                    source => "Anonymous"));

        CreateMap<Movie, MovieDto>()
            .ForCtorParam(
                "reviews",
                options => options.MapFrom(
                    source => source.Reviews))
            .ForCtorParam(
                "genres",
                options => options.MapFrom(
                    source => source.Genres
                        .Select(genre => genre.Name)
                        .ToList()));

        CreateMap<Review, AddReviewDto>()
            .ForMember(
                destination => destination.rating,
                options => options.MapFrom(
                    source => source.Rating));

        CreateMap<User, CreateUserDto>();

    }
}