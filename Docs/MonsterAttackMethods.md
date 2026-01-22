# Monster Attack Methods

The following pre-made attack methods for monsters are available.

> **Note:** Methods can be found at the bottom of `Server.Library/MonsterObject.cs`.  
> All methods are virtual so can be overridden.

## Available Methods

### PoisonTarget
```csharp
PoisonTarget(int chanceToPoison, long poisonDuration, PoisonType poison, long poisonTickSpeed = 1000)
```

### LineAttack
```csharp
LineAttack(int distance, int additionalDelay = 500)
```

### HalfmoonAttack
```csharp
HalfmoonAttack(int delay = 500)
```

### RangeAttack
```csharp
RangeAttack(int minAttackStat, int maxAttackStat, byte rangeAttackTypeNumber = 0, DefenceType type = DefenceType.MACAgility, int additionalDelay = 500)
```

### ArrowAttack
```csharp
ArrowAttack(int minAttackStat, int maxAttackStat, DefenceType type = DefenceType.MACAgility, int additionalDelay = 500)
```

### SinglePushAttack
```csharp
SinglePushAttack(int minAttackStat, int MaxAttackStat, DefenceType type = DefenceType.AC, int delay = 500)
```
