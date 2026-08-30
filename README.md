# SharpBotz
## Arena
### Creating A Simple Arena
Construct an `Arena` by calling the static `Create` method,
which takes an `ArenaWidth` and an `ArenaHeight` as arguments:   
```csharp
Arena.Create(
    ArenaWidth.Is(3),
    ArenaHeight.Is(3));
```
This creates a 3 by 3 grid.  
The outer tiles are set up as *Walls*  
```csharp
Wall Wall Wall
Wall      Wall
Wall Wall Wall
```
Both `ArenaWidth` and `ArenaHeight` must be greater than 2  
Walls can be added in the following way:  
```csharp
Wall
Wall
Wall
Wall
Wall
Wall
Wall
Wall
Wall
Wall
Wall
Wall
Wall
Wall
```
