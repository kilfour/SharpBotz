using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Rotators;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.C_PoweredModules;

[DocFile]
public class C_RotatorTests
{
    [Fact]
    [DocContent(
    """
    A rotator is needed in order to turn your bot.


    It is created by passing in its torque per power, maximum power and rotation along with a ModuleId (supplied as string).
    """)]
    [DocExample(typeof(C_RotatorTests), nameof(ConstructionExample))]
    public void Construction()
    {
        var rotator = ConstructionExample();
        Assert.Equal("rotator", rotator.Id.ToString());
    }

    [CodeSnippet]
    private static Rotator ConstructionExample() =>
        Rotator.Named("rotator")
            .TorquePerPower(10)
            .MaximumPower(15)
            .Left();

    [Fact]
    [DocContent(
    """
    Multiple rotators can be installed in the same ModuleRack.
    Each rotator has its own direction and ModuleId.
    """)]
    [DocExample(typeof(C_RotatorTests), nameof(MultipleExample))]
    public void MultipleRotators()
    {
        var rack = MultipleExample();
        var rotators = rack.GetModuleControl().FindModules<RotatorInfo>();

        Assert.Collection(
            rotators,
            rotator =>
            {
                Assert.IsType<LeftRotatorInfo>(rotator);
                Assert.Equal("left-rotator", rotator.Id.ToString());
            },
            rotator =>
            {
                Assert.IsType<RightRotatorInfo>(rotator);
                Assert.Equal("right-rotator", rotator.Id.ToString());
            });
    }

    [CodeSnippet]
    private static ModuleRack MultipleExample() =>
        ModuleRack.Create(
            Rotator.Named("left-rotator")
                .TorquePerPower(10)
                .MaximumPower(1)
                .Left(),
            Rotator.Named("right-rotator")
                .TorquePerPower(10)
                .MaximumPower(1)
                .Right());

    [Fact]
    [DocContent(
    """
    Supplying enough power can rotate a bot more than once in a single turn.
    Here the bot starts facing up and turns right twice, ending up facing down.
    """)]
    [DocExample(typeof(C_RotatorTests), nameof(CreateTurningWorld))]
    public void TurnTwice()
    {
        var world = CreateTurningWorld();

        world.Update();

        Assert.Equal(Direction.Down, world.Bots[0].Facing);
    }

    [CodeSnippet]
    private static GameWorld CreateTurningWorld() =>
        new(
            Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build(),
            [
                new BotState(
                    new Bot(
                        new TurnTwiceBrain(),
                        ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(2),
                            Battery.Named("battery").Capacity(10),
                            Rotator.Named("rotator")
                                .TorquePerPower(100)
                                .MaximumPower(2)
                                .Right())),
                    new Position(1, 1),
                    Direction.Up)
            ]);

    [Fact]
    [DocContent(
    """
    A rotator's base weight is 3.
    Supporting more power adds weight following the triangular number curve.
    """)]
    [DocBarChart(
        typeof(C_RotatorTests),
        nameof(MaximumPowerWeightCurve),
        "Weight by maximum power",
        "Maximum Power",
        "Weight",
        0, 18)]
    [DocContent(
    """
    Torque per power up to 10 is included in that weight.
    Above 10, every two additional torque per power add 1 weight, rounded up.
    """)]
    [DocBarChart(
        typeof(C_RotatorTests),
        nameof(TorquePerPowerWeightCurve),
        "Weight by torque per power",
        "Torque Per Power",
        "Weight",
        0, 7)]
    public void WeightCurveTest()
    {
        Assert.Equal(4, CreateRotator(torquePerPower: 10, maximumPower: 1).Weight);
        Assert.Equal(6, CreateRotator(torquePerPower: 10, maximumPower: 2).Weight);
        Assert.Equal(9, CreateRotator(torquePerPower: 10, maximumPower: 3).Weight);
        Assert.Equal(13, CreateRotator(torquePerPower: 10, maximumPower: 4).Weight);
        Assert.Equal(18, CreateRotator(torquePerPower: 10, maximumPower: 5).Weight);

        Assert.Equal(5, CreateRotator(torquePerPower: 11, maximumPower: 1).Weight);
        Assert.Equal(5, CreateRotator(torquePerPower: 12, maximumPower: 1).Weight);
        Assert.Equal(6, CreateRotator(torquePerPower: 13, maximumPower: 1).Weight);
        Assert.Equal(6, CreateRotator(torquePerPower: 14, maximumPower: 1).Weight);
        Assert.Equal(7, CreateRotator(torquePerPower: 15, maximumPower: 1).Weight);
    }

    private static readonly (int MaximumPower, int Weight)[] MaximumPowerWeightCurve =
        [
            (1, 4),
            (2, 6),
            (3, 9),
            (4, 13),
            (5, 18)
        ];

    private static readonly (int TorquePerPower, int Weight)[] TorquePerPowerWeightCurve =
        [
            (10, 4),
            (11, 5),
            (12, 5),
            (13, 6),
            (14, 6),
            (15, 7)
        ];

    private static Rotator CreateRotator(int torquePerPower, int maximumPower) =>
        Rotator.Named("rotator")
            .TorquePerPower(torquePerPower)
            .MaximumPower(maximumPower)
            .Left();

    private class TurnTwiceBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var reactor = modules.RequireModule<ReactorInfo>();
            var rotator = modules.RequireModule<RightRotatorInfo>();

            return new(
                reactor.SetOutput(2),
                new PowerAllocation(rotator.Id, 2));
        }
    }
}
