using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.D_Gameworld.A_Construction;

[DocFile]
public class A_CreatingAGameWorld
{
    [Fact]
    [DocContent(
    """
    A game world is created from a scenario containing its arena terrain and initial bot placements.
    A seed can be supplied to make the game repeatable.
    """)]
    [DocExample(typeof(A_CreatingAGameWorld), nameof(CreateGameWorld))]
    [DocContent("A newly created game starts at turn zero.")]
    public void Construction()
    {
        var world = CreateGameWorld();

        Assert.Equal(3, world.Arena.Width);
        Assert.Equal(3, world.Arena.Height);
        var botState = Assert.Single(world.Bots);
        Assert.IsType<DummyBot>(botState.Bot);
        Assert.Equal(new Position(1, 1), botState.Position);
        Assert.Equal(Direction.Right, botState.Facing);
        Assert.Equal(1234, world.Seed);
        Assert.Equal(0, world.Turn);
    }

    [CodeSnippet]
    private static GameWorld CreateGameWorld() =>
        Scenario.Named("Repeatable game")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build())
            .Spawn(() => new DummyBot())
                .At(1, 1)
                .Facing(Direction.Right)
            .CreateWorld(seed: 1234);
}
