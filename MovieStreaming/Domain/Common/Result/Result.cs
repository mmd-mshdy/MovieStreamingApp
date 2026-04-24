using MovieStreaming.Domain.Common.Errors;

namespace MovieStreaming.Domain.Common.Result
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error? Error { get; }
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
                throw new InvalidOperationException("Result Object invalid");
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error)

        {
            if (error == null || error == Error.None)
                throw new ArgumentException("Unknown problem occured");
            return new Result(false, error);
        }
        public static Result<T> Success<T>(T value) => new(value, true, Error.None);
        public static Result<T> Failure<T>(Error error)
        {
            if (error == null || error == Error.None)
                throw new InvalidOperationException("unknown problem occured");
            return new(default!, false, error);
        }
        public static   Result<T> Create<T>(T value) => value is not null ? Success(value) : Failure<T>(Error.None);

    }



}

