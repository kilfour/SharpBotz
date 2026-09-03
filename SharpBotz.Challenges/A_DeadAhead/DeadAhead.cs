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
                    ArenaWidth.Is(15),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(20)
            .CompletesWhen(world =>
                world.Bots.Count(bot => bot.Bot.IsAlive) < 2)
            .Spawn(() => new ChallengeBot()).At(1, 1).Facing(Direction.Right)
            .Spawn(() => new DummyBot()).At(13, 1).Facing(Direction.Right);
}

