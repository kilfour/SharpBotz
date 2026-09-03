using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Scenarios;

namespace SharpBotz.Challenges.B_DifferentRoutes;

public class DifferentRoutes
{
    public static Scenario RouteOne =>
        Scenario.Named("Route One")
            .Arena(
                Arena.Sized(
                    ArenaWidth.Is(7),
                    ArenaHeight.Is(7))
                    .AddWallAt(2, 1)
                    .AddWallAt(2, 2)
                    .AddWallAt(2, 3)
                    .AddWallAt(2, 5)
                    .AddWallAt(4, 1)
                    .AddWallAt(4, 3)
                    .AddWallAt(4, 4)
                    .AddWallAt(4, 5)
                .Build())
            .MaximumTurns(20)
            .CompletesWhen(world =>
                world.Bots.Count(bot => bot.Bot.IsAlive) < 2)
            .Spawn(() => new RouteBot()).At(1, 1).Facing(Direction.Right)
            .Spawn(() => new DummyBot()).At(5, 5).Facing(Direction.Right);

    public static Scenario RouteTwo =>
        Scenario.Named("Route Two")
            .Arena(
                Arena.Sized(
                    ArenaWidth.Is(7),
                    ArenaHeight.Is(7))
                    .AddWallAt(1, 2)
                    .AddWallAt(2, 2)
                    .AddWallAt(3, 2)
                    .AddWallAt(5, 2)
                    .AddWallAt(1, 4)
                    .AddWallAt(3, 4)
                    .AddWallAt(4, 4)
                    .AddWallAt(5, 4)
                .Build())
            .MaximumTurns(20)
            .CompletesWhen(world =>
                world.Bots.Count(bot => bot.Bot.IsAlive) < 2)
            .Spawn(() => new RouteBot()).At(1, 1).Facing(Direction.Right)
            .Spawn(() => new DummyBot()).At(5, 5).Facing(Direction.Right);
}

