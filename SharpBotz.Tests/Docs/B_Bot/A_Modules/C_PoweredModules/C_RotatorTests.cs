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
    [DocCode(
    """
    xychart-beta
        title "Weight by maximum power"
        x-axis "Maximum Power" [1, 2, 3, 4, 5]
        y-axis "Weight" 0 --> 18
        bar [4, 6, 9, 13, 18]
    """, "mermaid")]
    [DocContent(
    """
    Torque per power up to 10 is included in that weight.
    Above 10, every two additional torque per power add 1 weight, rounded up.
    """)]
    [DocCode(
    """
    xychart-beta
        title "Weight by torque per power"
        x-axis "Torque Per Power" [10, 11, 12, 13, 14, 15]
        y-axis "Weight" 0 --> 7
        bar [4, 5, 5, 6, 6, 7]
    """, "mermaid")]
    public void WeightCurve()
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
