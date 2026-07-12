using AutoMapper;
using MovieStreaming.Domain.Aggregates.Movies;
using MovieStreaming.Domain.Aggregates.Users;
namespace MovieStreaming.Application.DTOs.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, CreateMovieDto>();
            CreateMap< CreateMovieDto,Movie>();

            // Inside MappingProfile.cs
            CreateMap<Movie, MovieDto>()
    .ForMember(dest => dest.reviews, opt => opt.MapFrom(src => src.Reviews))
    // Mapping the genres collection to a list of strings
    .ForMember(dest => dest.genres, opt => opt.MapFrom(src =>
        src.Genres.Select(g => g.Name).ToList()));

            CreateMap<Review, ReviewDto>(); CreateMap<Review, AddReviewDto>().ForMember(dest => dest.rating ,opt => opt.MapFrom(src => 0.0));
            CreateMap<User, CreateUserDto>();
            CreateMap<CastMembers , CastMemberDto>();
        }
    }
}
