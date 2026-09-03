using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.G_UnderFire;

public class UnderFire : Challenge
{
    public static Scenario FireFromTheEast =>
        Scenario.Named("Under Fire - From The East")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(11),
                    ArenaHeight.Is(5))
                .Build())
            .MaximumTurns(20)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new UnderFireBot())
                .At(1, 2)
                .Facing(Direction.Right)
            .Spawn(() => new SentryBot(range: 8, damage: 10))
                .At(9, 2)
                .Facing(Direction.Left);

    public static Scenario FireFromTheNorth =>
        Scenario.Named("Under Fire - From The North")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(7),
                    ArenaHeight.Is(11))
                .Build())
            .MaximumTurns(20)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new UnderFireBot())
                .At(3, 9)
                .Facing(Direction.Up)
            .Spawn(() => new SentryBot(range: 8, damage: 10))
                .At(3, 1)
                .Facing(Direction.Down);
}
