using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Scenarios;

namespace SharpBotz.Tests.Docs.C_Scenario;

[DocFile]
public class ScenarioTests
{
    [Fact]
    [DocContent("A scenario describes repeatable initial arena terrain and bot placement.")]
    public void ScenarioIsReusable()
    {
        var scenario = Scenario.Named("Botz")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build())
            .Spawn(() => new Bot(new DummyBrain(), ModuleRack.Create()))
                .At(1, 1)
                .Facing(Direction.Up);

        var first = scenario.Start();
        var second = scenario.Start();

        Assert.Same(first.Arena, second.Arena);
        Assert.NotSame(first.Bots[0].Bot, second.Bots[0].Bot);
    }
}