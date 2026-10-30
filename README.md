# SWeetJelly Shared

SWeetJelly Shared contains small common value types that can be reused across SWeetJelly packages.

The shared package owns common contract shapes, not package-specific meanings.

```text
SWeetJelly.Shared
  Result
    Result
    Result<T>
    Results
    Error
  Policy
    Verdict
    Verdict<TReason>
    Verdict<TValue, TReason>
```

## Result

`Result` represents whether an operation succeeded or failed.

Use it when the caller needs to know if work completed successfully.

```csharp
using SWeetJelly.Shared.Result;

Result Save()
{
    if (HasSaveSlot())
        return Result.Success();

    return Result.Failure(SaveErrors.NoSlot);
}
```

## Result<T>

`Result<T>` represents whether an operation succeeded and, if it did, carries the produced value.

Use it when success has a value.

```csharp
using SWeetJelly.Shared.Result;

Result<int> LoadCoins()
{
    if (TryReadCoins(out var coins))
        return Result<int>.Success(coins);

    return Result<int>.Failure(SaveErrors.MissingCoins);
}
```

Access `Value` only after checking success.

```csharp
var result = LoadCoins();

if (result.IsFailure)
{
    UnityEngine.Debug.LogError(result.Error);
    return;
}

var coins = result.Value;
```

## Results

`Results` is a convenience factory for shorter call sites.

```csharp
using SWeetJelly.Shared.Result;

return Results.Success();
return Results.Success(player);
return Results.Failure(PlayerErrors.NotFound);
return Results.Failure<Player>(PlayerErrors.NotFound);
```

## Error

`Error` describes why an operation failed.

It is intended for failures, exceptions, invalid state, missing data, or domain errors.

Shared owns the `Error` type. Each package owns its concrete error catalog.

```csharp
using SWeetJelly.Shared.Result;

public static class SaveErrors
{
    public static readonly Error NoSlot =
        new Error("Save.NoSlot", "No save slot is available.");

    public static readonly Error MissingCoins =
        new Error("Save.MissingCoins", "Coins could not be loaded.");
}
```

## Policy

Policy types represent decisions, not operation failures.

A policy can evaluate successfully and still reject an action.

```csharp
using SWeetJelly.Shared.Policy;

Verdict<DungeonEntryRejectReason> CanEnterDungeon(Player player)
{
    if (player.Level >= 10)
        return Verdict<DungeonEntryRejectReason>.Allow();

    return Verdict<DungeonEntryRejectReason>.Reject(DungeonEntryRejectReason.LevelTooLow);
}

public enum DungeonEntryRejectReason
{
    None = 0,
    LevelTooLow = 1,
    DungeonClosed = 2
}
```

## Verdict

`Verdict` represents a policy decision without a shared reason payload.

Use it only when the caller needs the allow/reject state and the reject reason is not part of the contract.

```csharp
var verdict = isReady ? Verdict.Allow() : Verdict.Reject();
```

## Verdict<TReason>

`Verdict<TReason>` represents a policy decision with a package-owned reject reason type.

Use it when the question is:

```text
Is this action allowed? If not, why was it rejected?
```

`TReason` should be owned by the package that owns the policy meaning.

```csharp
public enum ShopPurchaseRejectReason
{
    None = 0,
    NotEnoughGold = 1,
    ItemSoldOut = 2
}
```

## Verdict<TValue, TReason>

`Verdict<TValue, TReason>` represents a policy decision that carries a value when allowed and a reason when rejected.

Use it when an allowed decision produces execution data.

```csharp
using SWeetJelly.Shared.Policy;

Verdict<IReadOnlyList<BoardCommand>, CommandRejectReason> ResolveCommand(Command command)
{
    if (!CanResolve(command))
        return Verdict<IReadOnlyList<BoardCommand>, CommandRejectReason>.Reject(CommandRejectReason.InvalidState);

    return Verdict<IReadOnlyList<BoardCommand>, CommandRejectReason>.Allow(BuildCommands(command));
}
```

Access `Value` only after checking `IsAllowed`.

```csharp
var verdict = ResolveCommand(command);

if (verdict.IsRejected)
    return;

Run(verdict.Value);
```

## Migration from 1.x

Version 2.0.0 removes all three deprecated Block factories. Replace Block() with Reject() and Block(reason) with Reject(reason), including the value-bearing Verdict form. IsBlocked is now deprecated; replace it with IsRejected. Its behavior remains unchanged until removal in a future major release.

## Result vs Verdict

`Result<T>` has `T` because operations often produce a value.

Examples:

```text
Load player -> Result<Player>
Parse config -> Result<Config>
Create save data -> Result<SaveData>
```

`Verdict<TReason>` has `TReason` because policy decisions often need a package-owned reject reason.

The policy question is usually binary:

```text
Allow or reject?
```

`Verdict<TValue, TReason>` adds an allowed value for decisions that produce execution data.

If a policy evaluation itself can fail, wrap the decision in `Result<Verdict<TReason>>` or `Result<Verdict<TValue, TReason>>`.

```csharp
using SWeetJelly.Shared.Policy;
using SWeetJelly.Shared.Result;

Result<Verdict<PolicyRejectReason>> EvaluatePolicy()
{
    if (!TryLoadPolicyConfig(out var config))
        return Results.Failure<Verdict<PolicyRejectReason>>(PolicyErrors.ConfigMissing);

    if (config.Enabled)
        return Results.Success(Verdict<PolicyRejectReason>.Allow());

    return Results.Success(Verdict<PolicyRejectReason>.Reject(PolicyRejectReason.Disabled));
}
```

In short:

```text
Result<T>                       = operation outcome with optional produced value
Error                           = operation failure shape
Verdict<TReason>                = policy decision with package-owned reject reason
Verdict<TValue, TReason>        = policy decision with allowed value or reject reason
Result<Verdict<TReason>>        = policy evaluation that can fail
Result<Verdict<TValue,TReason>> = value-producing policy evaluation that can fail
```

## Support and compatibility

Minimum supported Editor: Unity 6.3 LTS. The previous Unity 2022.3 floor is no longer supported by this migration. Verification results must name the actual Editor patch and platform; the support declaration does not claim every platform was tested.

Use factories to create intentional outcomes. Default Result values report failure with an empty Error; default verdicts report rejection with a default reason. Result<T>.Value throws on failure, while the value-bearing Verdict returns its default payload on rejection. Check state before consuming a payload. Null/default success payloads are supported; a reason-bearing Reject requires a nondefault reason.

Version 2.0.0 removes deprecated Block factories and retains IsBlocked as a deprecated alias for IsRejected. See [architecture][architecture] and the [compatibility decision][compatibility] for ownership, default-state behavior and migration policy. The Unity support-floor change and Block removal are breaking changes in 2.0.0.

[architecture]: ARCHITECTURE.md
[compatibility]: docs/adr/0001-support-and-api-compatibility.md