# Ranged
A ranged weapon fires in the direction your bot is facing.
The first bot in its path is hit, provided it is within range and no wall blocks the shot.


It is created with its range, damage per power and maximum power, along with a ModuleId.  
```csharp
Ranged.Named("ranged")
    .Range(3)
    .DamagePerPower(10)
    .MaximumPower(5);
```
Call `Fire` on the module info from your BotBrain to request a shot.
The requested damage is rounded up to the next whole unit of power.  
Supplying more than the ranged weapon's maximum power overcharges it.
The shot still lands, but every excess unit of power deals 3 damage to the attacking bot.  
A ranged weapon's base weight is 3.
Increasing range adds weight following the triangular number curve.  
```mermaid
xychart-beta
    title "Weight by range"
    x-axis "Range" [1, 2, 3, 4, 5]
    y-axis "Weight" 0 --> 20
    bar [6, 8, 11, 15, 20]
```
Higher damage per power adds weight following a squared curve.
This example uses a range of 1 and maximum power of 1.  
```mermaid
xychart-beta
    title "Weight by damage per power"
    x-axis "Damage Per Power" [1, 5, 10, 15, 20]
    y-axis "Weight" 0 --> 9
    bar [6, 6, 6, 8, 9]
```
Supporting more power adds weight following the triangular number curve.
This example uses a range of 3 and damage per power of 20.  
```mermaid
xychart-beta
    title "Weight by maximum power"
    x-axis "Maximum Power" [1, 2, 3, 4, 5]
    y-axis "Weight" 0 --> 28
    bar [14, 16, 19, 23, 28]
```
