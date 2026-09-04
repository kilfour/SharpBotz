using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Scanners;
using SharpBotz.Scenarios;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.C_PoweredModules;

[DocFile]
public class F_ScannerTests
{
    [Fact]
    [DocContent(
    """
    A scanner lets your bot observe a square area around itself on the following turn.


    It is created with the power required per unit of range and its maximum power, along with a ModuleId.
    """)]
    [DocExample(typeof(F_ScannerTests), nameof(ConstructionExample))]
    public void Construction()
    {
        var scanner = ConstructionExample();
        var info = (ScannerInfo)scanner.GetInfo(totalWeight: 0);

        Assert.Equal("scanner", scanner.Id.ToString());
        Assert.Equal(2, info.PowerPerRange);
        Assert.Equal(10, info.MaximumPower);
        Assert.Equal(5, info.MaximumRange);
    }

    [CodeSnippet]
    private static Scanner ConstructionExample() =>
        Scanner.Named("scanner")
            .PowerPerRange(2)
            .MaximumPower(10);

    [Fact]
    [DocContent(
    """
    Call `Scan` on the module info from your BotBrain to request a scan.
    Its power consumption is the requested range multiplied by power per range.
    """)]
    public void PowerConsumption()
    {
        var info = (ScannerInfo)ConstructionExample().GetInfo(totalWeight: 0);

        Assert.Equal(2, info.Scan(1).Power);
        Assert.Equal(6, info.Scan(3).Power);
        Assert.Equal(10, info.Scan(5).Power);
    }

    [Fact]
    [DocContent(
    """
    Supplying more than the scanner's maximum power overcharges it.
    The scan is still available on the following turn, but every excess unit of power deals 3 damage to the bot.
    """)]
    public void Overcharge()
    {
        var world = CreateOverchargedWorld();
        var brain = Assert.IsType<OneShotScanningBrain>(world.Bots[0].Bot.Brain);

        world.Update();
        world.Update();

        Assert.Equal(97, world.Bots[0].Bot.HitPoints);
        Assert.Equal(2, brain.Scans[1].Range);
        Assert.Equal(5, brain.Scans[1].Size);
        Assert.Equal(
            new ScanResult.OwnBot(Direction.Up, HitPoints: 97),
            brain.Scans[1][0, 0]);
    }

    [Fact]
    [DocContent(
    """
    Scan coordinates are relative to the observing bot, which is always at `[0, 0]`.
    Positive Y points ahead, positive X points to the bot's right, negative Y points behind, and negative X points to its left.
    A range-two scan therefore covers coordinates from `[-2, -2]` through `[2, 2]`.
    """)]
    [DocExample(typeof(F_ScannerTests), nameof(ReadOwnBot))]
    public void ScanCoordinatesAreRelativeToTheBot()
    {
        var world = CreateOverchargedWorld();
        var brain = Assert.IsType<OneShotScanningBrain>(world.Bots[0].Bot.Brain);

        world.Update();
        world.Update();

        Assert.Equal(
            new ScanResult.OwnBot(Direction.Up, HitPoints: 97),
            ReadOwnBot(brain.Scans[1]));
        Assert.IsType<ScanResult.Wall>(brain.Scans[1][-2, 0]);
        Assert.IsType<ScanResult.Wall>(brain.Scans[1][0, -2]);
    }

    [CodeExample]
    public static ScanResult ReadOwnBot(BotScan scan) =>
        scan[0, 0];

    [Fact]
    [DocContent(
    """
    A scan rotates with the observing bot. `[0, 1]` is therefore always directly ahead, regardless of the bot's arena direction.
    The `Facing` value in a bot scan result remains an absolute arena direction.

    In this example the observer faces left. A target one arena tile above it is on the observer's right and therefore appears at `[1, 0]`:
    """)]
    [DocExample(typeof(F_ScannerTests), nameof(ReadTileToTheBotsRight))]
    public void ScanOrientationTurnsWithTheBot()
    {
        var world = CreateOrientedScanWorld();
        var brain = Assert.IsType<OneShotScanningBrain>(world.Bots[0].Bot.Brain);

        world.Update();
        world.Update();

        var scan = brain.Scans[1];
        Assert.Equal(
            new ScanResult.OwnBot(Direction.Left, HitPoints: 100),
            scan[0, 0]);
        Assert.Equal(
            new ScanResult.Bot(Direction.Up, HitPoints: 100),
            ReadTileToTheBotsRight(scan));
    }

    [CodeExample]
    public static ScanResult ReadTileToTheBotsRight(BotScan scan) =>
        scan[1, 0];

    private static GameWorld CreateOverchargedWorld() =>
        Scenario.Named("Overcharged scanner")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(5))
                .Build())
            .MaximumTurns(2)
            .CompletesWhen(_ => false)
            .Spawn(() => Bot.Named("scanner")
                    .Brain(new OneShotScanningBrain(range: 2))
                    .Rack(ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(2),
                            Battery.Named("battery").Capacity(10),
                            Scanner.Named("scanner")
                                .PowerPerRange(1)
                                .MaximumPower(1))))
                .At(2, 2)
                .Facing(Direction.Up)
            .CreateWorld();

    private static GameWorld CreateOrientedScanWorld() =>
        new(
            Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(5))
                .Build(),
            [
                CreateScannerState(
                    new OneShotScanningBrain(range: 1),
                    x: 2,
                    y: 2,
                    facing: Direction.Left),
                CreateIdleState(x: 2, y: 1, facing: Direction.Up),
            ],
            maximumTurns: 2,
            complete: _ => false,
            seed: 1234);

    private static BotState CreateScannerState(
        BotBrain brain,
        int x,
        int y,
        Direction facing) =>
        new(
            Bot.Named("scanner")
                .Brain(brain)
                .Rack(ModuleRack.Create(
                    Reactor.Named("reactor").MaximumOutput(1),
                    Scanner.Named("scanner")
                        .PowerPerRange(1)
                        .MaximumPower(1))),
            new Position(x, y),
            facing);

    private static BotState CreateIdleState(
        int x,
        int y,
        Direction facing) =>
        new(
            Bot.Named("target")
                .Brain(new IdleBrain())
                .Rack(ModuleRack.Create()),
            new Position(x, y),
            facing);

    [Fact]
    [DocContent(
    """
    A scanner's base weight is 2.
    Supporting a larger maximum range adds weight following the triangular number curve.
    """)]
    [DocBarChart(
        typeof(F_ScannerTests),
        nameof(MaximumRangeWeightCurve),
        "Weight by maximum range",
        "Maximum Range",
        "Weight",
        0, 17)]
    [DocContent(
    """
    A scanner's standard efficiency is 3 power per range.
    Reducing the required power adds weight. This example keeps maximum range at 5.
    """)]
    [DocBarChart(
        typeof(F_ScannerTests),
        nameof(PowerPerRangeWeightCurve),
        "Weight by power per range",
        "Power Per Range",
        "Weight",
        0, 19)]
    public void WeightCurveTest()
    {
        foreach (var (maximumRange, weight) in MaximumRangeWeightCurve)
        {
            Assert.Equal(weight, CreateScanner(powerPerRange: 3, maximumRange).Weight);
        }

        foreach (var (powerPerRange, weight) in PowerPerRangeWeightCurve)
        {
            Assert.Equal(weight, CreateScanner(powerPerRange, maximumRange: 5).Weight);
        }
    }

    private static readonly (int MaximumRange, int Weight)[] MaximumRangeWeightCurve =
        [
            (1, 3),
            (2, 5),
            (3, 8),
            (4, 12),
            (5, 17)
        ];

    private static readonly (int PowerPerRange, int Weight)[] PowerPerRangeWeightCurve =
        [
            (1, 19),
            (2, 18),
            (3, 17),
            (4, 17),
            (5, 17)
        ];

    private static Scanner CreateScanner(int powerPerRange, int maximumRange) =>
        Scanner.Named("scanner")
            .PowerPerRange(powerPerRange)
            .MaximumPower(powerPerRange * maximumRange);

    private class OneShotScanningBrain(int range) : BotBrain
    {
        public List<BotScan> Scans { get; } = [];

        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            Scans.Add(observation.Scan);
            if (Scans.Count > 1)
            {
                return PowerPlan.Empty;
            }

            var scan = modules.RequireModule<ScannerInfo>().Scan(range);
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(scan.Power),
                scan);
        }
    }

    private class IdleBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
