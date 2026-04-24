using MovieStreaming.Domain.Common.Errors;

namespace MovieStreaming.Domain.Common.Result
{
    public class Result<T> : Result
    {
        private readonly T _value;
        internal protected Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }
        public static implicit operator Result<T>(T value)
        => Create(value);
        public static implicit operator Result<T>(Error error)
         => Failure<T>(error);
        public T Value => IsSuccess ? _value! : throw new InvalidOperationException("Value not accessible");

        public static implicit operator T(Result<T> result)
    => result.IsSuccess
        ? result.Value
        : throw new InvalidOperationException(
            $"Cannot convert failed Result<{typeof(T).Name}> to value. Error: {result.Error}"
        );

    }

}
