using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.I_MovingTargets;

public class MovingTargets : Challenge
{
    public static Scenario SinglePatrol =>
        Scenario.Named("Moving Targets - Single Patrol")
            .Arena(CreateArena())
            .MaximumTurns(40)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new MovingTargetBot())
                .At(1, 1)
                .Facing(Direction.Right)
            .Spawn(() => new PatrolBot())
                .At(3, 3)
                .Facing(Direction.Right);

    public static Scenario TwoPatrols =>
        Scenario.Named("Moving Targets - Two Patrols")
            .Arena(CreateArena())
            .MaximumTurns(50)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new MovingTargetBot())
                .At(1, 1)
                .Facing(Direction.Right)
            .Spawn(() => new PatrolBot())
                .At(3, 3)
                .Facing(Direction.Right)
            .Spawn(() => new PatrolBot())
                .At(7, 7)
                .Facing(Direction.Left);

    private static Arena CreateArena() =>
        Arena.Sized(
                ArenaWidth.Is(11),
                ArenaHeight.Is(11))
            .Build();
}
