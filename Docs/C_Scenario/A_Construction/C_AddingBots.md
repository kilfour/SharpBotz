# Adding Bots
This can be achieved in the following way:  
```csharp
Scenario.Named("Botz")
    .Arena(Arena.Sized(
            ArenaWidth.Is(5),
            ArenaHeight.Is(3))
        .Build())
    .MaximumTurns(20)
    .CompletesWhen(_ => false)
    .Spawn(() => new DummyBot())
        .At(1, 1)
        .Facing(Direction.Up)
    .Spawn(() => new DummyBot())
        .At(3, 1)
        .Facing(Direction.Up);
```
This creates:  
```text
Wall Wall Wall Wall Wall
Wall  ↑↑        ↑↑  Wall
Wall Wall Wall Wall Wall
```
