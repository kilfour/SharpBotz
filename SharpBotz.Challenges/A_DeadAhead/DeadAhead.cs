using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.A_DeadAhead;

public class DeadAhead
{
    public static Scenario Challenge =>
        Scenario.Named("Dead Ahead")
            .Arena(
                Arena.Sized(
                    ArenaWidth.Is(7),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(20)
            .CompletesWhen(a => a.Bots.Count < 2)
            .Spawn(() => new ChallengeBot()).At(1, 1).Facing(Direction.Right)
            .Spawn(() => new DummyBot()).At(5, 1).Facing(Direction.Right);
}

