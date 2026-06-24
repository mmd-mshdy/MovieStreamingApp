namespace MovieStreaming.Domain.Aggregates.Movies
{
    public class Genre : AggregateRoot
    {
        public string Name { get; private set; }

        private Genre() { }

        public Genre(string name)
        {
            Name = name;
        }
    }
}
