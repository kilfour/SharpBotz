# Bot
A bot combines three things:

- a name that identifies it in the game world;
- a `BotBrain` that decides what it wants to do each turn; and
- a `ModuleRack` containing the hardware available to that brain.

Every bot starts with 100 hit points. It remains alive while it has at least one hit point.  
## Assembling A Bot
Use `Bot.Named`, `Brain`, and `Rack` to supply the three required parts in order.
An empty module rack is valid and still contains the standard chassis, but a bot without modules cannot act:  
```csharp
Bot.Named("starter")
    .Brain(new IdleBrain())
    .Rack(ModuleRack.Create());
```
`IdleBrain` is the smallest valid brain. Returning an empty power plan makes the bot do nothing:  
```csharp
public class IdleBrain : BotBrain
{
    protected override PowerPlan RoutePower(
        ModuleControl modules,
        BotObservation observation) =>
        PowerPlan.Empty;
}
```
The bot exposes its name, brain, and rack after construction.
Module racks and bot brains are covered in detail in the following sections.  
## Adding A Module
Install modules by passing them to `ModuleRack.Create`.
Every module has a unique ID and adds its own weight to the standard chassis:  
```csharp
ModuleRack.Create(
    Reactor.Named("reactor")
        .MaximumOutput(3));
```
Weight is an important design constraint. A scenario can reject a bot that exceeds its maximum weight, and a heavier bot requires more power from its thrusters to move.

Call `ModuleRack.GetModuleWeights()` to inspect a rack's configuration. It returns a dictionary that maps each module ID to that module's weight:  
```csharp
rack.GetModuleWeights();
```
In this configuration the reactor weighs 6 and the chassis weighs 10, giving the rack a total weight of 16.  
## Defining A Reusable Bot Type
For a bot that will be created repeatedly, derive a class from `Bot` and pass the same three parts to its base constructor.
`nameof` keeps the displayed bot name in sync with the class name:  
```csharp
public class StarterBot() : Bot(
    nameof(StarterBot),
    new IdleBrain(),
    ModuleRack.Create());
```
Construct a fresh brain, rack, and set of modules for every bot instance.
A module rack belongs to one bot and cannot be shared by several bots.  
