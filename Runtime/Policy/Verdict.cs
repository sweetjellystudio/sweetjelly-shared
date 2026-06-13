namespace SWeetJelly.Shared.Policy
{
    public readonly struct Verdict
    {
        private Verdict(bool isAllowed)
        {
            IsAllowed = isAllowed;
        }

        public bool IsAllowed { get; }
        public bool IsBlocked => !IsAllowed;

        public static Verdict Allow()
        {
            return new Verdict(true);
        }

        public static Verdict Block()
        {
            return new Verdict(false);
        }
    }
}
