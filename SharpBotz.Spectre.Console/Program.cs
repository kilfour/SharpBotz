using QuickFuzzr;
using QuickPulse.Show;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Rotators;
using SharpBotz.Botz.BotModules.Scanners;
using SharpBotz.Botz.BotModules.Thrusters;
using SharpBotz.Scenarios;
using SharpBotz.Spectre;

var scenario = Scenario.Named("First contact")
    .Arena(
        Arena.Sized(
            ArenaWidth.Is(60),
            ArenaHeight.Is(20)))
    .MaximumTurns(200)
    .CompletesWhen(a => a.Bots.Count(a => a.Bot.IsAlive) < 2)
    .RandomWalls(30)
    .Spawn(() => CreateDuelist("Duelist One")).AtRandom()
    .Spawn(() => CreateDuelist("Duelist Two")).AtRandom();

var world = scenario.CreateWorld();
var display = new SpectreGameDisplay();
await display.RunAsync(world, scenario.Name);

static Bot CreateDuelist(string name) =>
    Bot.Named(name)
        .Brain(new DuelistBrain())
        .Rack(ModuleRack.Create(
            Reactor.Named("reactor").MaximumOutput(5),
            Battery.Named("battery").Capacity(10),
            Thruster.Named("thruster")
                .ThrustPerPower(50)
                .MaximumPower(2),
            Rotator.Named("left-rotator")
                .TorquePerPower(100)
                .MaximumPower(1)
                .Left(),
            Rotator.Named("right-rotator")
                .TorquePerPower(100)
                .MaximumPower(1)
                .Right(),
            Melee.Named("melee")
                .DamagePerPower(20)
                .MaximumPower(1),
            Scanner.Named("scanner")
                .PowerPerRange(1)
                .MaximumPower(1)));

class DuelistBrain : BotBrain
{
    protected override PowerPlan RoutePower(
        ModuleControl modules,
        BotObservation observation)
    {
        var scan = modules.RequireModule<ScannerInfo>().Scan(1);
        var movement = modules.RequireModule<ThrusterInfo>().Move(1);

        var ahead = observation.Scan[0, 1];

        if (ahead is ScanResult.Bot)
        {
            var attack = modules.RequireModule<MeleeInfo>().Hit(20);
            var requiredPower = attack.Power + scan.Power;
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(requiredPower),
                attack, scan);
        }
        if (ahead is ScanResult.Wall)
        {
            var turn = Generate(
                Fuzzr.OneOf<RotatorInfo>(
                    modules.RequireModule<LeftRotatorInfo>(),
                    modules.RequireModule<RightRotatorInfo>()))
                .Turn(1);
            var requiredPower = turn.Power + scan.Power;
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(requiredPower),
                turn, scan);
        }
        if (ahead is ScanResult.Empty)
        {
            var requiredPower = movement.Power + scan.Power;
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(requiredPower),
                movement, scan);
        }
        return PowerPlan.From(
            modules.RequireModule<ReactorInfo>().SetOutput(scan.Power),
            scan);
    }
}
