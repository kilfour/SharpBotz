# Creating A Game World
A game world is created from a scenario containing its arena terrain and initial bot placements.
A seed can be supplied to make the game repeatable.  
```csharp
Scenario.Named("Repeatable game")
    .Arena(Arena.Sized(
            ArenaWidth.Is(3),
            ArenaHeight.Is(3))
        .Build())
    .MaximumTurns(20)
    .CompletesWhen(_ => false)
    .Spawn(() => new DummyBot())
        .At(1, 1)
        .Facing(Direction.Right)
    .CreateWorld(seed: 1234);
```
A newly created game starts at turn one.  
