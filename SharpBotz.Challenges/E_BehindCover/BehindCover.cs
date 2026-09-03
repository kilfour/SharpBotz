using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.E_BehindCover;

public class BehindCover : Challenge
{
    public static Scenario LowerPassage =>
        Create(
            "Behind Cover - Lower Passage",
            new Position(4, 1),
            new Position(4, 2),
            new Position(4, 3),
            new Position(4, 4));

    public static Scenario UpperPassage =>
        Create(
            "Behind Cover - Upper Passage",
            new Position(4, 2),
            new Position(4, 3),
            new Position(4, 4),
            new Position(4, 5));

    private static Scenario Create(string name, params Position[] walls)
    {
        var arena = Arena.Sized(
            ArenaWidth.Is(9),
            ArenaHeight.Is(7));
        foreach (var wall in walls)
        {
            arena.AddWallAt(wall.X, wall.Y);
        }

        return Scenario.Named(name)
            .Arena(arena.Build())
            .MaximumTurns(40)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new CoverBot())
                .At(1, 3)
                .Facing(Direction.Right)
            .Spawn(() => new DummyBot())
                .At(7, 3)
                .Facing(Direction.Left);
    }
}
