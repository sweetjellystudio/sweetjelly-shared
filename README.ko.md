# SWeetJelly Shared

SWeetJelly Shared는 SWeetJelly 패키지들이 함께 재사용할 수 있는 작은 공통 value type을 담습니다.

Shared 패키지는 패키지별 구체적 의미가 아니라 공통 contract shape를 소유합니다.

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
```

## Result

`Result`는 operation이 성공했는지 실패했는지를 나타냅니다.

호출자가 작업 완료 여부를 알아야 할 때 사용합니다.

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

`Result<T>`는 operation 성공 여부와, 성공했을 때 생성된 값을 함께 나타냅니다.

성공 결과에 값이 있을 때 사용합니다.

```csharp
using SWeetJelly.Shared.Result;

Result<int> LoadCoins()
{
    if (TryReadCoins(out var coins))
        return Result<int>.Success(coins);

    return Result<int>.Failure(SaveErrors.MissingCoins);
}
```

`Value`는 반드시 성공 여부를 확인한 뒤 접근하세요.

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

`Results`는 호출부를 짧게 만들기 위한 factory입니다.

```csharp
using SWeetJelly.Shared.Result;

return Results.Success();
return Results.Success(player);
return Results.Failure(PlayerErrors.NotFound);
return Results.Failure<Player>(PlayerErrors.NotFound);
```

## Error

`Error`는 operation이 왜 실패했는지 설명합니다.

failure, exception, invalid state, missing data, domain error 등을 표현하기 위한 타입입니다.

Shared는 `Error` 타입을 소유합니다. 각 패키지는 자기 구체 error catalog를 소유합니다.

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

Policy 타입은 operation failure가 아니라 판단 결과를 나타냅니다.

policy 평가는 성공했지만 action은 block될 수 있습니다.

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

`Verdict`는 reason payload가 없는 policy decision을 나타냅니다.

호출자가 allow/block 상태만 필요하고 block reason이 contract에 포함되지 않을 때 사용합니다.

```csharp
var verdict = isReady ? Verdict.Allow() : Verdict.Block();
```

## Verdict<TReason>

`Verdict<TReason>`은 package-owned reason type을 포함하는 policy decision입니다.

다음 질문을 표현할 때 사용합니다.

```text
이 action은 허용되는가? 허용되지 않는다면 왜 block되었는가?
```

`TReason`은 policy 의미를 소유하는 패키지가 소유해야 합니다.

```csharp
public enum ShopPurchaseBlockReason
{
    None = 0,
    NotEnoughGold = 1,
    ItemSoldOut = 2
}
```

## Result vs Verdict

`Result<T>`에 `T`가 있는 이유는 operation이 값을 생성하는 경우가 많기 때문입니다.

예시:

```text
Load player -> Result<Player>
Parse config -> Result<Config>
Create save data -> Result<SaveData>
```

`Verdict<TReason>`에 `TReason`이 있는 이유는 policy decision이 package-owned blocked reason을 필요로 하는 경우가 많기 때문입니다.

policy 질문은 보통 binary입니다.

```text
Allow or block?
```

policy evaluation 자체가 실패할 수 있다면 decision을 `Result<Verdict<TReason>>`로 감싸세요.

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

요약:

```text
Result<T>                 = optional produced value를 가진 operation outcome
Error                     = operation failure shape
Verdict<TReason>          = package-owned reason을 가진 policy decision
Result<Verdict<TReason>>  = 실패할 수 있는 policy evaluation
```
