# Example Module Racks
Every module rack includes a chassis weighing 10, even when no modules are installed:  
```csharp
ModuleRack.Create();
```
This rack weighs **10**.  
A small mobile rack combines a one-power reactor with a drive.
Its high thrust efficiency lets it move the loaded chassis using that single unit of power:  
```csharp
ModuleRack.Create(
    Reactor.Named("reactor").MaximumOutput(1),
    Drive.Named("drive")
        .ThrustPerPower(100)
        .MaximumPower(1));
```
This rack weighs **63**.  
A close-combat rack can move, scan its surroundings, and strike an adjacent bot.
Its battery stores unused reactor output for later turns:  
```csharp
ModuleRack.Create(
    Reactor.Named("reactor").MaximumOutput(3),
    Battery.Named("battery").Capacity(10),
    Drive.Named("drive")
        .ThrustPerPower(100)
        .MaximumPower(1),
    Melee.Named("melee")
        .DamagePerPower(20)
        .MaximumPower(1),
    Scanner.Named("scanner")
        .PowerPerRange(1)
        .MaximumPower(1));
```
This rack weighs **80**, leaving 20 weight available for future upgrades.  
