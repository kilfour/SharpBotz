# Arena Definition
```csharp
Scenario.Named("My Scenario")
    .Arena(arena)
    .MaximumTurns(20)
    .CompletesWhen(_ => false);
```
