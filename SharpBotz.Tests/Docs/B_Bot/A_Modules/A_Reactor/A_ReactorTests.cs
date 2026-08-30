using QuickPulse.Explains;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Reactors;

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
        Assert.Equal(10, reactor.CurrentOutput);
        Assert.Equal(10, reactor.OutputPerTurn);
    }

    [CodeSnippet]
    private static Reactor ConstructionExample() =>
         Reactor.Create("reactor", 10);

    [Fact]
    [DocContent("A reactor with a maximum output of zero or negative throws upon construction.")]
    public void Invalid()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Reactor.Create("reactor", 0));
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => Reactor.Create("reactor", -1));
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
    [DocCode(
"""
xychart-beta
    title "Weight Curve"
    x-axis "Maximum Output" [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
    y-axis "Weight" 0 --> 6
    bar [3, 3, 3, 3, 3, 4, 4, 5, 6, 6]
""", "mermaid")]
    public void WeightCurve()
    {
        var reactor = Reactor.Create("reactor", 1);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Create("reactor", 2);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Create("reactor", 3);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Create("reactor", 4);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Create("reactor", 5);
        Assert.Equal(3, reactor.Weight);
        reactor = Reactor.Create("reactor", 6);
        Assert.Equal(4, reactor.Weight);
        reactor = Reactor.Create("reactor", 7);
        Assert.Equal(4, reactor.Weight);
        reactor = Reactor.Create("reactor", 8);
        Assert.Equal(5, reactor.Weight);
        reactor = Reactor.Create("reactor", 9);
        Assert.Equal(6, reactor.Weight);
        reactor = Reactor.Create("reactor", 10);
        Assert.Equal(6, reactor.Weight);
    }

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
        Assert.Equal(20, rack.ReactorOutput);
    }

    [CodeSnippet]
    private static ModuleRack MultipleExample() =>
        ModuleRack.Create(
            Reactor.Create("reactor-one", 10),
            Reactor.Create("reactor-two", 10));
}