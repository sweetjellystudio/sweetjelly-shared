namespace SWeetJelly.Shared.Policy
{
    using System;
    using System.Collections.Generic;

    public readonly struct Verdict<TReason>
    {
        private Verdict(bool isAllowed, TReason reason)
        {
            if (isAllowed && !EqualityComparer<TReason>.Default.Equals(reason, default))
                throw new ArgumentException("An allowed verdict cannot contain a block reason.", nameof(reason));

            if (!isAllowed && EqualityComparer<TReason>.Default.Equals(reason, default))
                throw new ArgumentException("A blocked verdict must contain a block reason.", nameof(reason));

            IsAllowed = isAllowed;
            Reason = reason;
        }

        public bool IsAllowed { get; }
        public bool IsBlocked => !IsAllowed;
        public TReason Reason { get; }

        public static Verdict<TReason> Allow()
        {
            return new Verdict<TReason>(true, default);
        }

        public static Verdict<TReason> Block(TReason reason)
        {
            return new Verdict<TReason>(false, reason);
        }
    }
}