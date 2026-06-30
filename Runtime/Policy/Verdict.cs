namespace SWeetJelly.Shared.Policy
{
    public readonly struct Verdict
    {
        private Verdict(bool isAllowed)
        {
            IsAllowed = isAllowed;
        }

        public bool IsAllowed { get; }
        public bool IsRejected => !IsAllowed;
        public bool IsBlocked => IsRejected;

        public static Verdict Allow()
        {
            return new Verdict(true);
        }

        public static Verdict Reject()
        {
            return new Verdict(false);
        }

        [System.Obsolete("Use Reject instead. Block will be removed in a future patch.")]
        public static Verdict Block()
        {
            return Reject();
        }
    }
}
