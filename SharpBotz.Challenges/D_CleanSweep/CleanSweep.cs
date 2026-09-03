using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.D_CleanSweep;

public class CleanSweep : Challenge
{
    public static Scenario CardinalPoints =>
        Create(
            "Clean Sweep - Cardinal Points",
            new Position(5, 2),
            new Position(8, 5),
            new Position(5, 8),
            new Position(2, 5));

    public static Scenario FourCorners =>
        Create(
            "Clean Sweep - Four Corners",
            new Position(2, 2),
            new Position(8, 2),
            new Position(8, 8),
            new Position(2, 8));

    private static Scenario Create(string name, params Position[] targets)
    {
        var scenario = Scenario.Named(name)
            .Arena(Arena.Sized(
                    ArenaWidth.Is(11),
                    ArenaHeight.Is(11))
                .Build())
            .MaximumTurns(45)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new SweepBot())
                .At(5, 5)
                .Facing(Direction.Up);

        foreach (var target in targets)
        {
            scenario.Spawn(() => new DummyBot())
                .At(target.X, target.Y)
                .Facing(Direction.Up);
        }

        return scenario;
    }
}
