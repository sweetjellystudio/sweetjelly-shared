namespace SWeetJelly.Shared.Policy
{
    using System;
    using System.Collections.Generic;

    public readonly struct Verdict<TReason>
    {
        private Verdict(bool isAllowed, TReason reason)
        {
            if (isAllowed && !EqualityComparer<TReason>.Default.Equals(reason, default))
                throw new ArgumentException("An allowed verdict cannot contain a reject reason.", nameof(reason));

            if (!isAllowed && EqualityComparer<TReason>.Default.Equals(reason, default))
                throw new ArgumentException("A rejected verdict must contain a reject reason.", nameof(reason));

            IsAllowed = isAllowed;
            Reason = reason;
        }

        public bool IsAllowed { get; }
        public bool IsRejected => !IsAllowed;
        [System.Obsolete("Use IsRejected instead. IsBlocked will be removed in a future major release.")]
        public bool IsBlocked => IsRejected;
        public TReason Reason { get; }
        public TReason RejectReason => Reason;

        public static Verdict<TReason> Allow()
        {
            return new Verdict<TReason>(true, default);
        }

        public static Verdict<TReason> Reject(TReason reason)
        {
            return new Verdict<TReason>(false, reason);
        }

    }
}
