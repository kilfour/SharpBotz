using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Scanners;
using SharpBotz.Scenarios;
using SharpBotz.Spectre;

var scenario = Scenario.Named("First contact")
    .Arena(
        Arena.Sized(
                ArenaWidth.Is(12),
                ArenaHeight.Is(7))
            .AddWallAt(6, 1)
            .AddWallAt(6, 5)
            .Build())
    .Spawn(CreateDuelist)
        .At(2, 3)
        .Facing(Direction.Right)
    .Spawn(CreateDuelist)
        .At(9, 3)
        .Facing(Direction.Left);

var world = scenario.Start();
var display = new SpectreGameDisplay();
await display.RunAsync(world, scenario.Name, maximumTurns: 20);

static Bot CreateDuelist() =>
    new(
        new DuelistBrain(),
        ModuleRack.Create(
            Reactor.Named("reactor").MaximumOutput(3),
            Battery.Named("battery").Capacity(10),
            Drive.Named("drive")
                .ThrustPerPower(100)
                .MaximumPower(1),
            new Melee(
                ModuleId.Is("melee"),
                damagePerPower: 20,
                maximumPower: 1),
            new Scanner(
                ModuleId.Is("scanner"),
                powerPerRange: 1,
                maximumPower: 1)));

sealed class DuelistBrain : BotBrain
{
    protected override PowerPlan RoutePower(
        ModuleControl modules,
        BotObservation observation)
    {
        var movement = modules.RequireModule<DrivingInfo>().Move(1);
        var attack = modules.RequireModule<MeleeInfo>().Hit(20);
        var scan = modules.RequireModule<ScannerInfo>().Scan(1);
        var requiredPower = movement.Power + attack.Power + scan.Power;

        return new(
            modules.RequireModule<ReactorInfo>().SetOutput(requiredPower),
            movement,
            attack,
            scan);
    }
}
