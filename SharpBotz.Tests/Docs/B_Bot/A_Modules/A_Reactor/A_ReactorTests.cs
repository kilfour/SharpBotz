using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Scenarios;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.B_Bot.A_Modules.A_Reactor;


[DocFile]
public class A_ReactorTests
{
    [Fact]
    [DocContent(
    """
A reactor is responsible for supplying the energy required to power other modules.

It is created by passing in it's maximum output along with a ModuleId (supplied as string).
""")]
    [DocExample(typeof(A_ReactorTests), nameof(ConstructionExample))]
    [DocContent("It's initial current output is set to maximum output.")]
    public void Construction()
    {
        var reactor = ConstructionExample();
        Assert.Equal(10, reactor.MaximumOutput);
    }

    [CodeSnippet]
    private static Reactor ConstructionExample() =>
         Reactor.Named("reactor")
            .MaximumOutput(10);

    [Fact]
    [DocContent("A reactor with a maximum output of zero or negative throws upon construction.")]
    public void Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Reactor.Named("reactor").MaximumOutput(0));
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Reactor.Named("reactor").MaximumOutput(-1));
        Assert.Equal("""
        outputPerTurn ('-1') must be a non-negative and non-zero value. (Parameter 'outputPerTurn')
        Actual value was -1.
        """, ex.Message);
    }

    [Fact]
    [DocContent(
"""
A reactor with a maximum output of 1 has a weight of 3.  
Increasing maximum output adds more weight exponentialy. 
"""
    )]
    [DocBarChart(
        typeof(A_ReactorTests),
        nameof(WeightCurve),
        "Weight Curve",
        "Maximum Output",
        "Weight",
        0, 6)]
    public void WeightCurveTest()
    {
        var reactor = Reactor.Named("reactor").MaximumOutput(1);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(2);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(3);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(4);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(5);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(6);
        Assert.Equal(4, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(7);
        Assert.Equal(4, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(8);
        Assert.Equal(5, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(9);
        Assert.Equal(6, reactor.Weight);
        reactor = Reactor.Named("reactor").MaximumOutput(10);
        Assert.Equal(6, reactor.Weight);
    }

    private static readonly (int MaximumOutput, int Weight)[] WeightCurve =
        [
            (1, 3),
            (2, 3),
            (3, 3),
            (4, 3),
            (5, 3),
            (6, 4),
            (7, 4),
            (8, 5),
            (9, 6),
            (10, 6)
        ];

    [Fact]
    [DocContent(
    """
    Requesting more than a reactor's maximum output overloads it.
    The reactor produces no power, and every excess unit of requested output deals 2 damage to the bot.

    Here a reactor with maximum output 1 is asked to generate 2 power, dealing 2 damage.
    """)]
    [DocExample(typeof(A_ReactorTests), nameof(CreateOverloadedWorld))]
    public void Overload()
    {
        var world = CreateOverloadedWorld();

        world.Update();

        Assert.Equal(98, world.Bots[0].Bot.HitPoints);
        Assert.Equal(0, world.Bots[0].Bot.ModuleRack.BatteryLevel);
    }

    [CodeSnippet]
    private static GameWorld CreateOverloadedWorld() =>
        Scenario.Named("Overloaded reactor")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build())
            .Spawn(() => new Bot(
                        new OverloadedReactorBrain(),
                        ModuleRack.Create(
                            Reactor.Named("reactor").MaximumOutput(1),
                            Battery.Named("battery").Capacity(10))))
                .At(1, 1)
                .Facing(Direction.Up)
            .CreateWorld();

    [Fact]
    [DocContent(
"""
Multiple rectors can be installed in a ModuleRack.

The total maximum output of the rack is then the sum of all reactors maximum outputs.
"""
    )]
    [DocExample(typeof(A_ReactorTests), nameof(MultipleExample))]
    public void Multiple()
    {
        var rack = MultipleExample();
        Assert.Equal(20, rack.MaximumReactorOutput);
    }

    [CodeSnippet]
    private static ModuleRack MultipleExample() =>
        ModuleRack.Create(
            Reactor.Named("reactor-one").MaximumOutput(10),
            Reactor.Named("reactor-two").MaximumOutput(10));

    private sealed class OverloadedReactorBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            new(modules.RequireModule<ReactorInfo>().SetOutput(2));
    }
}
