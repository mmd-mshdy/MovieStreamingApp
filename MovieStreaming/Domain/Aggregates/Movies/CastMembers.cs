using MovieStreaming.Domain.Common;
using MovieStreaming.Domain.Enums;

namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class CastMembers : Entity
    {
        public string Name {  get; set; }
        public string FamilyName {  get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public CastPositon castPositon { get; set; }
        public DateTime? Birthdate { get; set; }
        public IEnumerable<Movie>? movies { get; set; }
        public CastMembers(Guid id, string name, string familyName, string description, bool isFavorite, CastPositon castPositon, DateTime? birthdate) : base(id)
        {
            Name = name;
            FamilyName = familyName;
            Description = description;
            IsFavorite = isFavorite;
            this.castPositon = castPositon;
            Birthdate = birthdate;
        }
    }
}
