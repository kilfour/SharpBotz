using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.RangedWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Worlds;

namespace SharpBotz.Tests;

public class GameWorldRangedEffectsTests
{
    [Fact]
    public void PoweredRangedWeaponHitsABotWithinRange()
    {
        var world = CreateWorld(
            CreateArena(),
            CreateState(1, 2, Direction.Right, fire: true),
            CreateState(4, 2, Direction.Up));

        world.Update();

        Assert.Equal(100, world.Bots[0].Bot.HitPoints);
        Assert.Equal(80, world.Bots[1].Bot.HitPoints);
    }

    [Fact]
    public void PoweredRangedWeaponMissesBotsOutsideItsRange()
    {
        var world = CreateWorld(
            CreateArena(),
            CreateState(1, 2, Direction.Right, fire: true),
            CreateState(5, 2, Direction.Up));

        world.Update();

        Assert.Equal(100, world.Bots[1].Bot.HitPoints);
    }

    [Fact]
    public void WallBlocksTheShot()
    {
        var arena = Arena.Sized(
                ArenaWidth.Is(7),
                ArenaHeight.Is(5))
            .AddWallAt(3, 2)
            .Build();
        var world = CreateWorld(
            arena,
            CreateState(1, 2, Direction.Right, fire: true),
            CreateState(4, 2, Direction.Up));

        world.Update();

        Assert.Equal(100, world.Bots[1].Bot.HitPoints);
    }

    [Fact]
    public void ShotStopsAtTheFirstOccupiedPosition()
    {
        var world = CreateWorld(
            CreateArena(),
            CreateState(1, 2, Direction.Right, fire: true),
            CreateState(2, 2, Direction.Up),
            CreateState(4, 2, Direction.Up));

        world.Update();

        Assert.Equal(80, world.Bots[1].Bot.HitPoints);
        Assert.Equal(100, world.Bots[2].Bot.HitPoints);
    }

    [Fact]
    public void OpposingLethalShotsLandInTheSameTurn()
    {
        var world = CreateWorld(
            CreateArena(),
            CreateState(1, 2, Direction.Right, fire: true),
            CreateState(4, 2, Direction.Left, fire: true));
        world.Bots[0].Bot.TakeDamage(80);
        world.Bots[1].Bot.TakeDamage(80);

        world.Update();

        Assert.False(world.Bots[0].Bot.IsAlive);
        Assert.False(world.Bots[1].Bot.IsAlive);
    }

    [Fact]
    public void MovementIsResolvedBeforeRangedAttacks()
    {
        var world = CreateWorld(
            CreateArena(),
            CreateState(1, 2, Direction.Right, move: true, fire: true),
            CreateState(5, 2, Direction.Up));

        world.Update();

        Assert.Equal(new Position(2, 2), world.Bots[0].Position);
        Assert.Equal(80, world.Bots[1].Bot.HitPoints);
    }

    private static Arena CreateArena() =>
        Arena.Sized(
                ArenaWidth.Is(7),
                ArenaHeight.Is(5))
            .Build();

    private static GameWorld CreateWorld(
        Arena arena,
        params BotState[] botStates) =>
        new(arena, botStates, seed: 1234);

    private static BotState CreateState(
        int x,
        int y,
        Direction facing,
        bool move = false,
        bool fire = false)
    {
        var modules = new List<BotModule>
        {
            Reactor.Named("reactor").MaximumOutput(move ? 2 : 1),
            Battery.Named("battery").Capacity(10),
            Ranged.Named("ranged")
                .Range(3)
                .DamagePerPower(20)
                .MaximumPower(1),
        };
        if (move)
        {
            modules.Add(
                Drive.Named("drive")
                    .ThrustPerPower(100)
                    .MaximumPower(1));
        }

        return new(
            new Bot(
                new CombatBrain(move, fire),
                ModuleRack.Create([.. modules])),
            new Position(x, y),
            facing);
    }

    private sealed class CombatBrain(bool move, bool fire) : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var intentions = new List<PowerModuleIntent>();

            if (move)
            {
                intentions.Add(modules.RequireModule<DrivingInfo>().Move(1));
            }

            if (fire)
            {
                intentions.Add(modules.RequireModule<RangedInfo>().Fire(20));
            }

            if (intentions.Count == 0)
            {
                return PowerPlan.Empty;
            }

            var requiredPower = intentions
                .OfType<PowerAllocation>()
                .Sum(allocation => allocation.Power);
            intentions.Insert(
                0,
                modules.RequireModule<ReactorInfo>().SetOutput(requiredPower));
            return new([.. intentions]);
        }
    }
}
