# Building A Bot Brain
A bot brain decides what its bot will do each turn.
Create one by inheriting from `BotBrain` and implementing `RoutePower`.

`ModuleControl` provides information about the bot's installed modules.
Calling a module action creates a power intention; it does not immediately perform that action.
Return those intentions together in a `PowerPlan`.

This brain asks its drive to move one tile, then asks its reactor to generate exactly the power that movement requires:  
```csharp
public class MoveForwardBrain : BotBrain
{
    protected override PowerPlan RoutePower(
        ModuleControl modules,
        BotObservation observation)
    {
        var reactor = modules.RequireModule<ReactorInfo>();
        var drive = modules.RequireModule<DrivingInfo>();
        var movement = drive.Move(speed: 1);

        return PowerPlan.From(
            reactor.SetOutput(movement.Power),
            movement);
    }
}
```
The game world calls the brain once per turn and resolves the returned plan.
`BotObservation` contains what the bot observed on the previous turn and can be used to make later brains react to their surroundings.  
A brain is installed in a bot together with the module rack it controls:  
```csharp
public static Bot CreateBot() =>
    Bot.Named("move-forward")
        .Brain(new MoveForwardBrain())
        .Rack(ModuleRack.Create(
            Reactor.Named("reactor").MaximumOutput(1),
            Drive.Named("drive")
                .ThrustPerPower(100)
                .MaximumPower(1)));
```
