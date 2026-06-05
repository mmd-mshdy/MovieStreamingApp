using MovieStreaming.Domain.Aggregates.Movies;

namespace MovieStreaming.Application.Interfaces
{
    public interface ICastMemberRepository
    {
        Task<CastMembers> GetByIdAsync (Guid id);
        Task<IEnumerable<CastMembers>> GetByFullNameAsync (string name , string familyname);
        Task<CastMembers> CreateAsync (CastMembers member);
        Task<CastMembers> UpdateAsync (CastMembers member);
        Task DeleteAsync(Guid id);

    }
}
