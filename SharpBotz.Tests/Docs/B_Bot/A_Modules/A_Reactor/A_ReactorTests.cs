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
        "Weight")]
    public void WeightCurveTest()
    {
        foreach (var (maximumOutput, weight) in WeightCurve)
        {
            Assert.Equal(weight, Reactor.Named("reactor").MaximumOutput(maximumOutput).Weight);
        }
    }

    private static readonly (int MaximumOutput, int Weight)[] WeightCurve =
        [
            (1, 4),
            (2, 5),
            (3, 6),
            (4, 7),
            (5, 8),
            (6, 10),
            (7, 11),
            (8, 13),
            (9, 15),
            (10, 16)
        ];

    [Fact]
    [DocContent(
    """
    Requesting more than a reactor's maximum output overloads it.
    The reactor produces no power, and every excess unit of requested output deals 2 damage to the bot.
    """)]
    public void Overload()
    {
        var world = CreateOverloadedWorld();

        world.Update();

        Assert.Equal(98, world.Bots[0].Bot.HitPoints);
        Assert.Equal(0, world.Bots[0].Bot.ModuleRack.BatteryLevel);
    }

    private static GameWorld CreateOverloadedWorld() =>
        Scenario.Named("Overloaded reactor")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(1)
            .CompletesWhen(_ => false)
            .Spawn(() => Bot.Named("overloaded-reactor")
                    .Brain(new OverloadedReactorBrain())
                    .Rack(ModuleRack.Create(
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

    private class OverloadedReactorBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.From(modules.RequireModule<ReactorInfo>().SetOutput(2));
    }
}
