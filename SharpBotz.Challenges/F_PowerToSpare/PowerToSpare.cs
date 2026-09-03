using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.F_PowerToSpare;

public class PowerToSpare : Challenge
{
    public static Scenario FiveInReserve =>
        Create("Power To Spare - Five In Reserve", minimumStoredPower: 5, targetX: 5);

    public static Scenario TenInReserve =>
        Create("Power To Spare - Ten In Reserve", minimumStoredPower: 10, targetX: 7);

    private static Scenario Create(
        string name,
        int minimumStoredPower,
        int targetX) =>
        Scenario.Named(name)
            .Arena(Arena.Sized(
                    ArenaWidth.Is(targetX + 2),
                    ArenaHeight.Is(3))
                .Build())
            .MaximumTurns(30)
            .CompletesWhen(world =>
                OnlyFirstBotLivesWithStoredPower(world, minimumStoredPower))
            .Spawn(() => new ReserveBot())
                .At(1, 1)
                .Facing(Direction.Right)
            .Spawn(() => new DummyBot())
                .At(targetX, 1)
                .Facing(Direction.Left);
}
