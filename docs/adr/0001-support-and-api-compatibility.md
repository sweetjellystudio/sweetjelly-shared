#   Unity 6.3 support and API migration compatibility

Accepted 2026-10-30. Version 2.0.0 establishes Unity 6.3 LTS as the support floor and removes the three Block factories deprecated since 1.1.0. Keeping the old Unity 2022.3 declaration would imply support this work does not verify. Actual tested Editor patches and platforms remain separate evidence.

The existing Block migration path has already been offered in 1.1.0, so callers now migrate Block() to Reject() and Block(reason) to Reject(reason), including the value-bearing Verdict. Keeping Block longer was considered, but the agreed major transition provides an opportunity to remove the already deprecated surface.

IsBlocked was not previously deprecated. Version 2.0.0 therefore marks it obsolete while preserving its behavior until a future major release; callers migrate to IsRejected. This separates removal of an existing migration path from introduction of a new one, retaining migration time at the cost of supporting and testing the alias.

Future public API deprecations require a minor or major version; incompatible removals require a major version. Publication, tags, commits and game-consumer upgrades remain outside this task. This decision does not claim verification of every platform or authorization to change unrelated contracts.