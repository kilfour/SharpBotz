# Creating A Simple Arena
Begin constructing an `Arena` by calling the static `Sized` method,
which takes an `ArenaWidth` and an `ArenaHeight`. Finish by calling `Build`:   
```csharp
Arena.Sized(
        ArenaWidth.Is(3),
        ArenaHeight.Is(3))
    .Build();
```
This creates a 3 by 3 grid.  
The outer tiles are set up as *Walls*  
```text
Wall Wall Wall
Wall      Wall
Wall Wall Wall
```
Both `ArenaWidth` and `ArenaHeight` must be greater than 2  
