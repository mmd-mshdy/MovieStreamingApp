public abstract class AggregateRoot
{
    public Guid Id { get; protected set; }

    protected AggregateRoot()
    {
        // EF Core uses this
    }

    protected AggregateRoot(Guid id)
    {
        Id = id;
    }
}
