#   Shared architecture

Shared owns small synchronous value types describing operation outcomes and policy decisions. It does not own consumer error catalogs, policy rules, storage, orchestration, or presentation. See the [usage guide][usage] for API examples and the [compatibility decision][compatibility] before changing support or public behavior.

##  Contracts and dependencies

`SWeetJelly.Shared.Result` owns `Error`, `Result`, `Result<T>` and `Results`. `SWeetJelly.Shared.Policy` owns the three `Verdict` forms. Both runtime assemblies have no assembly dependencies and disable Unity engine references. They are independent peers; neither requires the other. Consumers can compose them as `Result<Verdict<...>>` when evaluating a policy can itself fail.

Result expresses operation success or failure. Verdict expresses allow or reject after a policy decision. A rejected decision is not automatically an operation failure. The consuming package owns concrete errors and rejection reasons.

##  Invariants and edge cases

Failure factories require a nonempty Error. Reason-bearing rejection factories require a reason unequal to its type's default value; for an enum, zero cannot be an explicit rejection reason. Success and allow factories accept default or null payloads.

`Result<T>.Value` throws on failure. `Verdict<TValue,TReason>.Value` is an unguarded property and returns default on rejection. Callers must check state before interpreting payloads. Reason and RejectReason are aliases; IsBlocked is a deprecated alias for IsRejected. The deprecated Block factories were removed in 2.0.0; consumers use Reject.

Default struct construction bypasses factories. `default(Result)` and `default(Result<T>)` report failure with an empty Error. Default verdicts report rejection with default reason and value. These existing states are preserved for compatibility; use factories for intentional outcomes. `default(Error)` has null string properties, whereas its constructor normalizes null strings to empty strings; both are empty according to IsEmpty.

Readonly structs do not make a referenced payload immutable or thread-safe. Shared performs no resource disposal, scheduling, cancellation, or Unity lifecycle work. Consumers own payload lifetime and synchronization.

##  Consumers and verification

Loading, serialization and save consume Result through their contract/application assemblies; save also references it in its Unity framework assembly. Shared remains upstream of those packages and does not reference them. Host integration checks catch downstream compilation problems; package EditMode tests cover value contracts and compatibility independently.

There are no serialized Unity assets, runtime components, or Inspector UI in this package. Manual Play Mode and Inspector acceptance are not required for this value-contract migration. README examples are the consumption guide; a new Samples~ UI would add no necessary evidence.

[usage]: README.md
[compatibility]: docs/adr/0001-support-and-api-compatibility.md
