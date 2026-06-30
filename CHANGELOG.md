# Changelog

## v1.1.0

updated at 2026-06-15

- [Added] `Verdict.Reject()` and `Verdict<TReason>.Reject(TReason)` for decision rejection terminology.
- [Added] `Verdict<TValue, TReason>` for allowed decisions that carry a value and rejected decisions that carry a reason.
- [Added] `IsRejected` and `RejectReason` aliases to verdict types.
- [Deprecated] `Block()` factory methods in favor of `Reject()`. They remain available for compatibility and will be removed in a future patch.

## v1.0.0

updated at 2026-06-05

- [Added] `Result` for success/failure operation outcomes.
- [Added] `Result<T>` for successful operation outcomes with values.
- [Added] `Results` convenience factory.
- [Added] `Error` for operation failure descriptions.
- [Added] `Verdict` for reasonless policy allow/block decisions.
- [Added] `Verdict<TReason>` for policy decisions with package-owned block reasons.
