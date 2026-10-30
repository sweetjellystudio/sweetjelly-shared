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
        [System.Obsolete("Use IsRejected instead. IsBlocked will be removed in a future major release.")]
        public bool IsBlocked => IsRejected;

        public static Verdict Allow()
        {
            return new Verdict(true);
        }

        public static Verdict Reject()
        {
            return new Verdict(false);
        }

    }
}
