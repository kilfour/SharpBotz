using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Worlds;

namespace SharpBotz.Tests;

public class GameWorldMeleeEffectsTests
{
    [Fact]
    public void PoweredMeleeWeaponHitsTheBotInFront()
    {
        var world = CreateWorld(
            CreateState(2, 2, Direction.Right, attack: true),
            CreateState(3, 2, Direction.Up));

        world.Update();

        Assert.Equal(100, world.Bots[0].Bot.HitPoints);
        Assert.Equal(80, world.Bots[1].Bot.HitPoints);
    }

    [Fact]
    public void PoweredMeleeWeaponMissesBotsOutsideTheTargetPosition()
    {
        var world = CreateWorld(
            CreateState(1, 2, Direction.Right, attack: true),
            CreateState(3, 2, Direction.Up));

        world.Update();

        Assert.Equal(100, world.Bots[1].Bot.HitPoints);
    }

    [Fact]
    public void BotDestroyedBeforeItsMeleeAttackDoesNotAttack()
    {
        var world = CreateWorld(
            CreateState(2, 2, Direction.Right, attack: true),
            CreateState(3, 2, Direction.Left, attack: true));
        world.Bots[0].Bot.TakeDamage(80);
        world.Bots[1].Bot.TakeDamage(80);

        world.Update();

        Assert.True(world.Bots[0].Bot.IsAlive);
        Assert.Equal(20, world.Bots[0].Bot.HitPoints);
        Assert.False(world.Bots[1].Bot.IsAlive);
    }

    [Fact]
    public void MovementIsResolvedBeforeMeleeAttacks()
    {
        var world = CreateWorld(
            CreateState(1, 2, Direction.Right, move: true, attack: true),
            CreateState(3, 2, Direction.Up));

        world.Update();

        Assert.Equal(new Position(2, 2), world.Bots[0].Position);
        Assert.Equal(80, world.Bots[1].Bot.HitPoints);
    }

    private static GameWorld CreateWorld(params BotState[] botStates) =>
        new(
            Arena.Sized(
                    ArenaWidth.Is(5),
                    ArenaHeight.Is(5))
                .Build(),
            botStates,
            maximumTurns: 10,
            complete: _ => false,
            seed: 1234);

    private static BotState CreateState(
        int x,
        int y,
        Direction facing,
        bool move = false,
        bool attack = false)
    {
        var modules = new List<BotModule>
        {
            Reactor.Named("reactor").MaximumOutput(move ? 2 : 1),
            Battery.Named("battery").Capacity(10),
            Melee.Named("melee")
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
            Bot.Named($"melee-{x}-{y}")
                .Brain(new CombatBrain(move, attack))
                .Rack(ModuleRack.Create([.. modules])),
            new Position(x, y),
            facing);
    }

    private class CombatBrain(bool move, bool attack) : BotBrain
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

            if (attack)
            {
                intentions.Add(modules.RequireModule<MeleeInfo>().Hit(20));
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
            return PowerPlan.From([.. intentions]);
        }
    }
}
