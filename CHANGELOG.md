# Changelog

## v2.0.0 (unreleased)

- [Changed] Minimum supported Unity Editor is now 6.3 LTS (6000.3); Unity 2022.3 support is discontinued.
- [Removed] Three Block factories deprecated since 1.1.0; migrate to Reject.
- [Deprecated] IsBlocked remains compatible; migrate to IsRejected before its removal in a future major release.
- [Added] Contract regression tests for Reject, value-bearing Verdict, null payloads and default states.
- [Added] Package agent instructions, architecture and compatibility decision documentation.

Version 2.0.0 is prepared locally; this section is not a published release. See README migration guidance.

## v1.1.0

updated at 2026-06-15

- [Added] `Verdict.Reject()` and `Verdict<TReason>.Reject(TReason)` for decision rejection terminology.
- [Added] `Verdict<TValue, TReason>` for allowed decisions that carry a value and rejected decisions that carry a reason.
- [Added] `IsRejected` and `RejectReason` aliases to verdict types.
- [Deprecated] `Block()` factory methods in favor of `Reject()`. They remain available for compatibility and will be removed in a future major release.

## v1.0.0

updated at 2026-06-05

- [Added] `Result` for success/failure operation outcomes.
- [Added] `Result<T>` for successful operation outcomes with values.
- [Added] `Results` convenience factory.
- [Added] `Error` for operation failure descriptions.
- [Added] `Verdict` for reasonless policy allow/block decisions.
- [Added] `Verdict<TReason>` for policy decisions with package-owned block reasons.
