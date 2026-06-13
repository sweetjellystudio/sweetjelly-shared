namespace SWeetJelly.Shared.Result
{
    using System;

#nullable enable

    public readonly struct Result<T>
    {
        private readonly T? value;

        private Result(T value)
        {
            this.value = value;
            IsSuccess = true;
            Error = Error.None;
        }

        private Result(Error error)
        {
            if (error.IsEmpty)
                throw new ArgumentException("A failed result must contain an error.", nameof(error));

            value = default;
            IsSuccess = false;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public T Value
        {
            get
            {
                if (IsFailure)
                    throw new InvalidOperationException("Cannot access the value of a failed result.");

                return value!;
            }
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Failure(Error error)
        {
            return new Result<T>(error);
        }
    }
}
