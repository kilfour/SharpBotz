using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.H_Crossfire;

public class Crossfire : Challenge
{
    public static Scenario ThreeWays => Create("Crossfire - Three Ways", includeWest: false);

    public static Scenario FourWays => Create("Crossfire - Four Ways", includeWest: true);

    private static Scenario Create(string name, bool includeWest)
    {
        var scenario = Scenario.Named(name)
            .Arena(Arena.Sized(
                    ArenaWidth.Is(11),
                    ArenaHeight.Is(11))
                .Build())
            .MaximumTurns(16)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new CrossfireBot())
                .At(5, 5)
                .Facing(Direction.Up)
            .Spawn(() => new SentryBot(range: 5, damage: 8))
                .At(5, 1)
                .Facing(Direction.Down)
            .Spawn(() => new SentryBot(range: 5, damage: 8))
                .At(9, 5)
                .Facing(Direction.Left)
            .Spawn(() => new SentryBot(range: 5, damage: 8))
                .At(5, 9)
                .Facing(Direction.Up);

        if (includeWest)
        {
            scenario.Spawn(() => new SentryBot(range: 5, damage: 8))
                .At(1, 5)
                .Facing(Direction.Right);
        }

        return scenario;
    }
}
