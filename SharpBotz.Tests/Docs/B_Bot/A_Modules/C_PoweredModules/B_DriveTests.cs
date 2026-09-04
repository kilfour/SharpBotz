using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Scenarios;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.C_PoweredModules;

[DocFile]
public class B_DriveTests
{
    [Fact]
    [DocContent(
    """
    A drive is needed to move your bot across the arena.


    It is created with its thrust per power and maximum power, along with a ModuleId.
    Thrust per power determines how much force each unit of supplied power produces.
    """)]
    [DocExample(typeof(B_DriveTests), nameof(ConstructionExample))]
    public void Construction()
    {
        var drive = ConstructionExample();
        var info = (DrivingInfo)drive.GetInfo(totalWeight: 50);

        Assert.Equal("drive", drive.Id.ToString());
        Assert.Equal(10, info.ThrustPerPower);
        Assert.Equal(5, info.MaximumPower);
        Assert.Equal(18, drive.Weight);
    }

    [CodeSnippet]
    private static Drive ConstructionExample() =>
        Drive.Named("drive")
            .ThrustPerPower(10)
            .MaximumPower(5);

    [Fact]
    [DocContent(
    """
    Call `Move` on the module info from your BotBrain to request a speed.
    The required power is the requested speed multiplied by the bot's loaded weight, divided by thrust per power and rounded up.

    For a bot weighing 50 with 10 thrust per power, every unit of speed needs 5 power.
    Requesting speed 2 allocates 10 power, which exceeds this drive's maximum power of 5.
    """)]
    public void PowerConsumption()
    {
        var drive = ConstructionExample();
        var info = (DrivingInfo)drive.GetInfo(totalWeight: 50);

        Assert.Equal(5, info.Move(1).Power);
        Assert.Equal(10, info.Move(2).Power);
        Assert.Equal(15, info.Move(3).Power);
        Assert.Equal(20, info.Move(4).Power);
    }

    [Fact]
    [DocContent(
    """
    A powered drive moves the bot in the direction it is facing.
    """)]
    public void MoveOneTile()
    {
        var world = CreateMovingWorld();

        world.Update();

        Assert.Equal(new Position(2, 1), world.Bots[0].Position);
    }

    private static GameWorld CreateMovingWorld() =>
        Scenario.Named("Moving one tile")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(1)
            .CompletesWhen(_ => false)
            .Spawn(() => Bot.Named("moving")
                    .Brain(new MoveRightBrain())
                    .Rack(ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(1),
                            Battery.Named("battery").Capacity(10),
                            Drive.Named("drive")
                                .ThrustPerPower(100)
                                .MaximumPower(1))))
                .At(1, 1)
                .Facing(Direction.Right)
            .CreateWorld();

    [Fact]
    [DocContent(
    """
    Supplying more than the drive's maximum power overcharges it.
    The movement still happens, but every excess unit of power deals 3 damage to the bot.
    """)]
    public void Overcharge()
    {
        var world = CreateOverchargedWorld();

        world.Update();

        Assert.Equal(new Position(2, 1), world.Bots[0].Position);
        Assert.Equal(97, world.Bots[0].Bot.HitPoints);
    }

    private static GameWorld CreateOverchargedWorld() =>
        Scenario.Named("Overcharged drive")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(1)
            .CompletesWhen(_ => false)
            .Spawn(() => Bot.Named("overcharged-drive")
                    .Brain(new MoveRightBrain())
                    .Rack(ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(2),
                            Battery.Named("battery").Capacity(10),
                            Drive.Named("drive")
                                .ThrustPerPower(20)
                                .MaximumPower(1))))
                .At(1, 1)
                .Facing(Direction.Right)
            .CreateWorld();

    [Fact]
    [DocContent(
    """
    A drive's base weight is 3.
    Supporting more power adds weight following the triangular number curve.
    """)]
    [DocBarChart(
        typeof(B_DriveTests),
        nameof(MaximumPowerWeightCurve),
        "Weight by maximum power",
        "Maximum Power",
        "Weight",
        0, 18)]
    [DocContent(
    """
    Thrust per power up to 10 is included in that weight.
    Above 10, every two additional thrust per power add 1 weight, rounded up.
    """)]
    [DocBarChart(
        typeof(B_DriveTests),
        nameof(ThrustPerPowerWeightCurve),
        "Weight by thrust per power",
        "Thrust Per Power",
        "Weight",
        0, 7)]
    public void WeightCurveTest()
    {
        foreach (var (maximumPower, weight) in MaximumPowerWeightCurve)
        {
            Assert.Equal(weight, CreateDrive(thrustPerPower: 10, maximumPower).Weight);
        }

        foreach (var (thrustPerPower, weight) in ThrustPerPowerWeightCurve)
        {
            Assert.Equal(weight, CreateDrive(thrustPerPower, maximumPower: 1).Weight);
        }
    }

    private static readonly (int MaximumPower, int Weight)[] MaximumPowerWeightCurve =
        [
            (1, 4),
            (2, 6),
            (3, 9),
            (4, 13),
            (5, 18)
        ];

    private static readonly (int ThrustPerPower, int Weight)[] ThrustPerPowerWeightCurve =
        [
            (10, 4),
            (11, 5),
            (12, 5),
            (13, 6),
            (14, 6),
            (15, 7)
        ];

    private static Drive CreateDrive(int thrustPerPower, int maximumPower) =>
        Drive.Named("drive")
            .ThrustPerPower(thrustPerPower)
            .MaximumPower(maximumPower);

    private class MoveRightBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var reactor = modules.RequireModule<ReactorInfo>();
            var drive = modules.RequireModule<DrivingInfo>();
            var movement = drive.Move(1);

            return PowerPlan.From(
                reactor.SetOutput(movement.Power),
                movement);
        }
    }
}
