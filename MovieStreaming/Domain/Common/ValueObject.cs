namespace MovieStreaming.Domain.Common
{
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public bool Equals(ValueObject? other)
        {
            return other != null && ValuesAreEqual(other);
        }
        public override int GetHashCode()
        {
            return GetAtomicValues().Aggregate(default(int), HashCode.Combine);
        }
        public override bool Equals(object? obj)
        {
            return obj is ValueObject other && ValuesAreEqual(other);
        }

        public abstract IEnumerable<object> GetAtomicValues();
        
        public bool ValuesAreEqual(ValueObject other)
        {
            return GetAtomicValues().SequenceEqual(other.GetAtomicValues()); 
        }
    }
}
