using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.D_Gameworld.A_Construction;

[DocFile]
public class A_CreatingAGameWorld
{
    [Fact]
    [DocContent(
    """
    A game world is created from immutable arena terrain and the initial state of its bots.
    A seed can be supplied to make the game repeatable.
    """)]
    [DocExample(typeof(A_CreatingAGameWorld), nameof(CreateGameWorld))]
    [DocContent("A newly created game starts at turn zero.")]
    public void Construction()
    {
        var arena =
            Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build();
        var botState = new BotState(
            new Bot(new DummyBrain(), ModuleRack.Create()),
            new Position(1, 1),
            Direction.Right);

        var world = CreateGameWorld(arena, botState);

        Assert.Same(arena, world.Arena);
        Assert.Same(botState, Assert.Single(world.Bots));
        Assert.Equal(1234, world.Seed);
        Assert.Equal(0, world.Turn);
    }

    [CodeSnippet]
    private static GameWorld CreateGameWorld(Arena arena, BotState botState) =>
        new(
            arena,
            [botState],
            seed: 1234);

    private sealed class DummyBrain : BotBrain
    {
        protected override PowerPlan RoutePower(BotObservation observation) =>
            PowerPlan.Empty;
    }
}
