using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Scenarios;

namespace SharpBotz.Tests.Docs.C_Scenario.A_Construction;

[DocFile]
public class A_ArenaDefinition
{
    [Fact]
    [DocExample(typeof(A_ArenaDefinition), nameof(GetScenario))]
    public void Construction()
    {
        var scenario = GetScenario();
        Assert.Equal("My Scenario", scenario.Name);
    }

    [CodeSnippet]
    private static Scenario GetScenario() =>
        Scenario.Named("My Scenario")
            .ArenaSize(
                ArenaWidth.Is(3),
                ArenaHeight.Is(3));
}

