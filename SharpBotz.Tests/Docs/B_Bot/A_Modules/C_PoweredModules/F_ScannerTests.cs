using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Scanners;
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
        new Scanner(
            ModuleId.Is("scanner"),
            powerPerRange: 2,
            maximumPower: 10);

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

    Here a scanner with maximum power 1 receives 2 power. It produces a range-2 scan and deals 3 damage.
    """)]
    [DocExample(typeof(F_ScannerTests), nameof(CreateOverchargedWorld))]
    public void Overcharge()
    {
        var world = CreateOverchargedWorld();
        var brain = Assert.IsType<OneShotScanningBrain>(world.Bots[0].Bot.Brain);

        world.Update();
        world.Update();

        Assert.Equal(97, world.Bots[0].Bot.HitPoints);
        Assert.Equal(5, brain.Scans[1].GetLength(0));
        Assert.Equal(5, brain.Scans[1].GetLength(1));
        Assert.Equal(
            new ScanResult.OwnBot(Direction.Up, HitPoints: 97),
            brain.Scans[1][2, 2]);
    }

    [CodeSnippet]
    private static GameWorld CreateOverchargedWorld() =>
        new GameWorld(
            Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(5))
                .Build(),
            [
                new BotState(
                    new Bot(
                        new OneShotScanningBrain(range: 2),
                        ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(2),
                            Battery.Named("battery").Capacity(10),
                            new Scanner(
                                ModuleId.Is("scanner"),
                                powerPerRange: 1,
                                maximumPower: 1))),
                    new Position(2, 2),
                    Direction.Up)
            ]);

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
        new(
            ModuleId.Is("scanner"),
            powerPerRange,
            maximumPower: powerPerRange * maximumRange);

    private sealed class OneShotScanningBrain(int range) : BotBrain
    {
        public List<ScanResult[,]> Scans { get; } = [];

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
            return new(
                modules.RequireModule<ReactorInfo>().SetOutput(scan.Power),
                scan);
        }
    }
}
