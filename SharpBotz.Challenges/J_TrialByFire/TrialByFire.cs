using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.J_TrialByFire;

public class TrialByFire : Challenge
{
    public static Scenario OpenArena =>
        Scenario.Named("Trial By Fire - Open Arena")
            .Arena(Arena.Sized(
                    ArenaWidth.Is(13),
                    ArenaHeight.Is(13))
                .Build())
            .MaximumTurns(50)
            .CompletesWhen(OnlyFirstBotLives)
            .Spawn(() => new TrialBot())
                .At(6, 6)
                .Facing(Direction.Up)
            .Spawn(() => new SentryBot(range: 6, damage: 8))
                .At(6, 1)
                .Facing(Direction.Down)
            .Spawn(() => new SentryBot(range: 6, damage: 8))
                .At(11, 6)
                .Facing(Direction.Left)
            .Spawn(() => new SentryBot(range: 6, damage: 8))
                .At(6, 11)
                .Facing(Direction.Up)
            .Spawn(() => new PatrolBot())
                .At(2, 2)
                .Facing(Direction.Right);

    public static Scenario BrokenGround
    {
        get
        {
            var arena = Arena.Sized(
                ArenaWidth.Is(13),
                ArenaHeight.Is(13));
            AddSplitWall(arena, x: 3);
            AddSplitWall(arena, x: 9);

            return Scenario.Named("Trial By Fire - Broken Ground")
                .Arena(arena.Build())
                .MaximumTurns(60)
                .CompletesWhen(OnlyFirstBotLives)
                .Spawn(() => new TrialBot())
                    .At(6, 6)
                    .Facing(Direction.Up)
                .Spawn(() => new SentryBot(range: 10, damage: 8))
                    .At(1, 6)
                    .Facing(Direction.Right)
                .Spawn(() => new SentryBot(range: 10, damage: 8))
                    .At(11, 6)
                    .Facing(Direction.Left)
                .Spawn(() => new PatrolBot())
                    .At(5, 2)
                    .Facing(Direction.Right);
        }
    }

    public static Scenario FinalArena
    {
        get
        {
            var arena = Arena.Sized(
                    ArenaWidth.Is(15),
                    ArenaHeight.Is(15))
                .AddWallAt(6, 5)
                .AddWallAt(6, 6)
                .AddWallAt(6, 8)
                .AddWallAt(6, 9)
                .AddWallAt(8, 5)
                .AddWallAt(8, 6)
                .AddWallAt(8, 8)
                .AddWallAt(8, 9)
                .Build();

            return Scenario.Named("Trial By Fire - Final Arena")
                .Arena(arena)
                .MaximumTurns(80)
                .CompletesWhen(OnlyFirstBotLives)
                .Spawn(() => new TrialBot())
                    .At(7, 7)
                    .Facing(Direction.Up)
                .Spawn(() => new SentryBot(range: 8, damage: 8))
                    .At(7, 1)
                    .Facing(Direction.Down)
                .Spawn(() => new SentryBot(range: 8, damage: 8))
                    .At(13, 7)
                    .Facing(Direction.Left)
                .Spawn(() => new SentryBot(range: 8, damage: 8))
                    .At(7, 13)
                    .Facing(Direction.Up)
                .Spawn(() => new SentryBot(range: 8, damage: 8))
                    .At(1, 7)
                    .Facing(Direction.Right)
                .Spawn(() => new PatrolBot())
                    .At(2, 2)
                    .Facing(Direction.Right)
                .Spawn(() => new PatrolBot())
                    .At(12, 12)
                    .Facing(Direction.Left);
        }
    }

    private static void AddSplitWall(Arena.ArenaBuilder arena, int x)
    {
        for (var y = 1; y < 12; y++)
        {
            if (y != 6)
                arena.AddWallAt(x, y);
        }
    }
}
