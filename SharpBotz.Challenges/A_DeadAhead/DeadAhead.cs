using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.A_DeadAhead;

public class DeadAhead : Challenge
{
    public static Scenario Challenge =>
        Scenario.Named("Dead Ahead")
            .Arena(
                Arena.Sized(
                    ArenaWidth.Is(15),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(20)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new AheadBot()).At(1, 1).Facing(Direction.Right)
            .Spawn(() => new DummyBot()).At(13, 1).Facing(Direction.Right);
}

