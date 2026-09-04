# Maximum Bot Weight
A Scenario can define the maximum bot weight allowed.  
```csharp
Scenario.Named("My Scenario")
    .Arena(arena)
    .MaximumTurns(20)
    .CompletesWhen(_ => false)
    .MaximumBotWeight(1);
```
Using the following bot in that scenario:  
```csharp
Bot.Named("Heavy")
    .Brain(new DummyBrain())
    .Rack(ModuleRack.Create(
        Drive.Named("drive")
            .ThrustPerPower(10)
            .MaximumPower(5)));
```
Causes `CreateWorld` to throw:  
```csharp
ArgumentException
```
```csharp
$"A bot cannot weigh more than 1. Heavy's module rack weighs 28. (Parameter 'placement')";
```
