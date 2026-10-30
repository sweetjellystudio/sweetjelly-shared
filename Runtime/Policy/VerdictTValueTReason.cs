namespace SWeetJelly.Shared.Policy
{
    using System;
    using System.Collections.Generic;

    public readonly struct Verdict<TValue, TReason>
    {
        private Verdict(bool isAllowed, TValue value, TReason reason)
        {
            if (isAllowed && !EqualityComparer<TReason>.Default.Equals(reason, default))
                throw new ArgumentException("An allowed verdict cannot contain a reject reason.", nameof(reason));

            if (!isAllowed && !EqualityComparer<TValue>.Default.Equals(value, default))
                throw new ArgumentException("A rejected verdict cannot contain a value.", nameof(value));

            if (!isAllowed && EqualityComparer<TReason>.Default.Equals(reason, default))
                throw new ArgumentException("A rejected verdict must contain a reject reason.", nameof(reason));

            IsAllowed = isAllowed;
            Value = value;
            Reason = reason;
        }

        public bool IsAllowed { get; }
        public bool IsRejected => !IsAllowed;
        [System.Obsolete("Use IsRejected instead. IsBlocked will be removed in a future major release.")]
        public bool IsBlocked => IsRejected;
        public TValue Value { get; }
        public TReason Reason { get; }
        public TReason RejectReason => Reason;

        public static Verdict<TValue, TReason> Allow(TValue value)
        {
            return new Verdict<TValue, TReason>(true, value, default);
        }

        public static Verdict<TValue, TReason> Reject(TReason reason)
        {
            return new Verdict<TValue, TReason>(false, default, reason);
        }

    }
}
