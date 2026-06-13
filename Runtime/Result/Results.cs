namespace SWeetJelly.Shared.Result
{
    public static class Results
    {
        public static Result Success()
        {
            return Result.Success();
        }

        public static Result Failure(Error error)
        {
            return Result.Failure(error);
        }

        public static Result<T> Success<T>(T value)
        {
            return Result<T>.Success(value);
        }

        public static Result<T> Failure<T>(Error error)
        {
            return Result<T>.Failure(error);
        }

        public static Error Error(string code, string message)
        {
            return new Error(code, message);
        }
    }
}
