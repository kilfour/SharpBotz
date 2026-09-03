using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.C_LongShot;

public class LongShot : Challenge
{
    private static readonly Position Start = new(4, 4);

    public static Scenario TargetEast =>
        Create("Long Shot - Target East", new Position(7, 4));

    public static Scenario TargetSouth =>
        Create("Long Shot - Target South", new Position(4, 7));

    public static Scenario TargetWest =>
        Create("Long Shot - Target West", new Position(1, 4));

    private static Scenario Create(string name, Position target) =>
        Scenario.Named(name)
            .Arena(Arena.Sized(
                    ArenaWidth.Is(9),
                    ArenaHeight.Is(9))
                .Build())
            .MaximumTurns(12)
            .CompletesWhen(world => OnlyFirstBotLivesAt(world, Start))
            .Spawn(() => new LongShotBot())
                .At(Start.X, Start.Y)
                .Facing(Direction.Up)
            .Spawn(() => new DummyBot())
                .At(target.X, target.Y)
                .Facing(Direction.Up);
}
