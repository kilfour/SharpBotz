using SharpBotz.Arenas;
using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.RangedWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Worlds;

namespace SharpBotz.Tests;

public class GameWorldCombatEffectsTests
{
    [Fact]
    public void MeleeAndRangedAttacksResolveInTheSameCombatPhase()
    {
        var meleeBot = CreateMeleeBot();
        var rangedBot = CreateRangedBot();
        var world = new GameWorld(
            Arena.Sized(ArenaWidth.Is(5), ArenaHeight.Is(5)).Build(),
            [
                new BotState(meleeBot, new Position(2, 2), Direction.Right),
                new BotState(rangedBot, new Position(3, 2), Direction.Left),
            ],
            maximumTurns: 10,
            complete: _ => false,
            seed: 1234);
        meleeBot.TakeDamage(80);
        rangedBot.TakeDamage(80);

        var events = world.Update().OfType<BotDamaged>().ToArray();

        Assert.False(meleeBot.IsAlive);
        Assert.False(rangedBot.IsAlive);
        Assert.Equal(2, events.Length);
        Assert.Contains(
            events,
            damaged =>
                ReferenceEquals(damaged.Bot, rangedBot) &&
                damaged.Cause is DamageCause.MeleeAttack);
        Assert.Contains(
            events,
            damaged =>
                ReferenceEquals(damaged.Bot, meleeBot) &&
                damaged.Cause is DamageCause.RangedAttack);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SimultaneousDamageAttributionDoesNotDependOnBotOrder(
        bool reverseAttackers)
    {
        var meleeBot = CreateMeleeBot();
        var rangedBot = CreateRangedBot(range: 2);
        var target = Bot.Named("target")
            .Brain(new IdleBrain())
            .Rack(ModuleRack.Create());
        var meleeState = new BotState(
            meleeBot,
            new Position(1, 2),
            Direction.Right);
        var rangedState = new BotState(
            rangedBot,
            new Position(4, 2),
            Direction.Left);
        var targetState = new BotState(
            target,
            new Position(2, 2),
            Direction.Up);
        var botStates = reverseAttackers
            ? new[] { rangedState, meleeState, targetState }
            : [meleeState, rangedState, targetState];
        var world = new GameWorld(
            Arena.Sized(ArenaWidth.Is(6), ArenaHeight.Is(5)).Build(),
            botStates,
            maximumTurns: 10,
            complete: _ => false,
            seed: 1234);
        target.TakeDamage(70);

        var damaged = Assert.IsType<BotDamaged>(Assert.Single(world.Update()));

        Assert.Equal(30, damaged.Damage);
        Assert.False(target.IsAlive);
        var combined = Assert.IsType<DamageCause.Combined>(damaged.Cause);
        Assert.Collection(
            combined.Contributions,
            contribution =>
            {
                Assert.Equal(20, contribution.Damage);
                var cause = Assert.IsType<DamageCause.MeleeAttack>(
                    contribution.Cause);
                Assert.Equal("melee", cause.Attacker.Name);
            },
            contribution =>
            {
                Assert.Equal(20, contribution.Damage);
                var cause = Assert.IsType<DamageCause.RangedAttack>(
                    contribution.Cause);
                Assert.Equal("ranged", cause.Attacker.Name);
            });
    }

    private static Bot CreateMeleeBot() =>
        Bot.Named("melee")
            .Brain(new MeleeBrain())
            .Rack(ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(1),
                Battery.Named("battery").Capacity(10),
                Melee.Named("melee").DamagePerPower(20).MaximumPower(1)));

    private static Bot CreateRangedBot(int range = 1) =>
        Bot.Named("ranged")
            .Brain(new RangedBrain())
            .Rack(ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(1),
                Battery.Named("battery").Capacity(10),
                Ranged.Named("ranged")
                    .Range(range)
                    .DamagePerPower(20)
                    .MaximumPower(1)));

    private class MeleeBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var attack = modules.RequireModule<MeleeInfo>().Hit(20);
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(attack.Power),
                attack);
        }
    }

    private class RangedBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation)
        {
            var attack = modules.RequireModule<RangedInfo>().Fire(20);
            return PowerPlan.From(
                modules.RequireModule<ReactorInfo>().SetOutput(attack.Power),
                attack);
        }
    }

    private class IdleBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
