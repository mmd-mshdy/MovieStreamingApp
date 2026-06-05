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

            CreateMap<Movie, MovieDto>();
            CreateMap<Review, AddReviewDto>().ForMember(dest => dest.rating ,opt => opt.MapFrom(src => 0.0));
            CreateMap<User, CreateUserDto>();
            CreateMap<CastMembers , CastMemberDto>();
        }
    }
}
