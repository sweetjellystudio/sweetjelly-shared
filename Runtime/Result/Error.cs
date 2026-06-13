namespace SWeetJelly.Shared.Result
{
#nullable enable

    public readonly struct Error
    {
        public static readonly Error None = new Error(string.Empty, string.Empty);

        public Error(string code, string message)
        {
            Code = code ?? string.Empty;
            Message = message ?? string.Empty;
        }

        public string Code { get; }
        public string Message { get; }
        public bool IsEmpty => string.IsNullOrWhiteSpace(Code) && string.IsNullOrWhiteSpace(Message);

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Code))
                return Message;

            if (string.IsNullOrWhiteSpace(Message))
                return Code;

            return $"{Code}: {Message}";
        }
    }
}
