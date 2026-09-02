using QuickPulse.Explains;
using SharpBotz.Arenas;
using SharpBotz.Scenarios;
using SharpBotz.Worlds;

namespace SharpBotz.Tests.Docs.D_Gameworld.B_Turns;

[DocFile]
public class A_AdvancingTurns
{
    [Fact]
    [DocContent("Updating the game world advances it by one turn.")]
    [DocExample(typeof(A_AdvancingTurns), nameof(AdvanceOneTurn))]
    public void UpdatingAdvancesTurns()
    {
        var world = CreateGameWorld();

        AdvanceOneTurn(world);

        Assert.Equal(2, world.Turn);

        AdvanceOneTurn(world);

        Assert.Equal(3, world.Turn);
    }

    [CodeSnippet]
    private static void AdvanceOneTurn(GameWorld world) =>
        world.Update();

    private static GameWorld CreateGameWorld() =>
        Scenario.Named("Advancing turns")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(3),
                    ArenaHeight.Is(3))
                .Build())
            .CreateWorld();
}
