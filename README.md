# SWeetJelly Shared

SWeetJelly Shared contains small common value types that can be reused across SWeetJelly packages.

The shared package owns common contract shapes, not package-specific meanings.

```text
SWeetJelly.Shared
├── Result
│   ├── Result
│   ├── Result<T>
│   ├── Results
│   └── Error
└── Policy
    ├── Verdict
    └── Verdict<TReason>
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

A policy can evaluate successfully and still block an action.

```csharp
using SWeetJelly.Shared.Policy;

Verdict<DungeonEntryBlockReason> CanEnterDungeon(Player player)
{
    if (player.Level >= 10)
        return Verdict<DungeonEntryBlockReason>.Allow();

    return Verdict<DungeonEntryBlockReason>.Block(DungeonEntryBlockReason.LevelTooLow);
}

public enum DungeonEntryBlockReason
{
    None = 0,
    LevelTooLow = 1,
    DungeonClosed = 2
}
```

## Verdict

`Verdict` represents a policy decision without a shared reason payload.

Use it only when the caller needs the allow/block state and the block reason is not part of the contract.

```csharp
var verdict = isReady ? Verdict.Allow() : Verdict.Block();
```

## Verdict<TReason>

`Verdict<TReason>` represents a policy decision with a package-owned reason type.

Use it when the question is:

```text
Is this action allowed? If not, why was it blocked?
```

`TReason` should be owned by the package that owns the policy meaning.

```csharp
public enum ShopPurchaseBlockReason
{
    None = 0,
    NotEnoughGold = 1,
    ItemSoldOut = 2
}
```

## Result vs Verdict

`Result<T>` has `T` because operations often produce a value.

Examples:

```text
Load player -> Result<Player>
Parse config -> Result<Config>
Create save data -> Result<SaveData>
```

`Verdict<TReason>` has `TReason` because policy decisions often need a package-owned blocked reason.

The policy question is usually binary:

```text
Allow or block?
```

If a policy evaluation itself can fail, wrap the decision in `Result<Verdict<TReason>>`.

```csharp
using SWeetJelly.Shared.Policy;
using SWeetJelly.Shared.Result;

Result<Verdict<PolicyBlockReason>> EvaluatePolicy()
{
    if (!TryLoadPolicyConfig(out var config))
        return Results.Failure<Verdict<PolicyBlockReason>>(PolicyErrors.ConfigMissing);

    if (config.Enabled)
        return Results.Success(Verdict<PolicyBlockReason>.Allow());

    return Results.Success(Verdict<PolicyBlockReason>.Block(PolicyBlockReason.Disabled));
}
```

In short:

```text
Result<T>                 = operation outcome with optional produced value
Error                     = operation failure shape
Verdict<TReason>          = policy decision with package-owned reason
Result<Verdict<TReason>>  = policy evaluation that can fail
```
