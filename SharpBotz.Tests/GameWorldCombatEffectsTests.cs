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

    private static Bot CreateMeleeBot() =>
        Bot.Named("melee")
            .Brain(new MeleeBrain())
            .Rack(ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(1),
                Battery.Named("battery").Capacity(10),
                Melee.Named("melee").DamagePerPower(20).MaximumPower(1)));

    private static Bot CreateRangedBot() =>
        Bot.Named("ranged")
            .Brain(new RangedBrain())
            .Rack(ModuleRack.Create(
                Reactor.Named("reactor").MaximumOutput(1),
                Battery.Named("battery").Capacity(10),
                Ranged.Named("ranged")
                    .Range(1)
                    .DamagePerPower(20)
                    .MaximumPower(1)));

    private sealed class MeleeBrain : BotBrain
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

    private sealed class RangedBrain : BotBrain
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
}
