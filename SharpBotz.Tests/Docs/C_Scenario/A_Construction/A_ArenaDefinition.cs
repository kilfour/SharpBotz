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
        var arena =
            Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build();
        var scenario = GetScenario(arena);
        Assert.Equal("My Scenario", scenario.Name);
        Assert.Same(arena, scenario.Arena);
    }

    [CodeSnippet]
    private static Scenario GetScenario(Arena arena) =>
        Scenario.Named("My Scenario")
            .Arena(arena);
}

