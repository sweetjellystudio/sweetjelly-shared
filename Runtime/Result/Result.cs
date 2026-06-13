namespace SWeetJelly.Shared.Result
{
    using System;

    public readonly struct Result
    {
        private Result(bool isSuccess, Error error)
        {
            if (isSuccess && !error.IsEmpty)
                throw new ArgumentException("A successful result cannot contain an error.", nameof(error));

            if (!isSuccess && error.IsEmpty)
                throw new ArgumentException("A failed result must contain an error.", nameof(error));

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success()
        {
            return new Result(true, Error.None);
        }

        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }
    }
}
