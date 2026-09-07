using SharpBotz.Arenas;
using SharpBotz.Botz.BotModules.RangedWeapons;

namespace SharpBotz.Worlds.EffectResolving;

public static class RangedEffectsResolver
{
    public static void Handle(
        Arena arena,
        BotStateEffect[] botStateEffects,
        ICollection<WorldEvent> worldEvents)
    {
        var pendingDamage = new List<PendingDamage>();
        HandleParticipants(
            arena,
            [.. botStateEffects.Where(effect => effect.BotState.Bot.IsAlive)],
            pendingDamage);
        DamageResolver.Handle(pendingDamage, worldEvents);
    }

    public static void HandleParticipants(
        Arena arena,
        IReadOnlyList<BotStateEffect> participants,
        ICollection<PendingDamage> pendingDamage)
    {
        var occupants = participants.ToLookup(botStateEffect =>
            botStateEffect.BotState.Position.ToCoordinates());

        foreach (var attacker in participants)
        {
            foreach (var effect in attacker.Effects.RangedEffects.OfType<RangedEffect>())
            {
                Fire(arena, occupants, attacker, effect, pendingDamage);
            }
            foreach (var effect in attacker.Effects.RangedEffects.OfType<RangedOverChargedEffect>())
            {
                pendingDamage.Add(new(
                    attacker.BotState.Bot,
                    effect.ExcessPower * 3,
                    new DamageCause.RangedOvercharged(effect.Id)));
            }
        }
    }

    private static void Fire(
        Arena arena,
        ILookup<(int X, int Y), BotStateEffect> occupants,
        BotStateEffect attacker,
        RangedEffect effect,
        ICollection<PendingDamage> pendingDamage)
    {
        var attackerState = attacker.BotState;
        var target = attackerState.Position;

        for (var distance = 1; distance <= effect.Range; distance++)
        {
            target = target.Move(attackerState.Facing);
            if (!arena.IsTraversable(target))
            {
                return;
            }

            var receivers = occupants[target.ToCoordinates()]
                .Where(receiver =>
                    !ReferenceEquals(attackerState.Bot, receiver.BotState.Bot))
                .ToArray();

            if (receivers.Length == 0)
            {
                continue;
            }

            foreach (var receiver in receivers)
            {
                pendingDamage.Add(new(
                    receiver.BotState.Bot,
                    effect.Damage,
                    new DamageCause.RangedAttack(attackerState.Bot, effect.Id)));
            }
            return;
        }
    }
}
