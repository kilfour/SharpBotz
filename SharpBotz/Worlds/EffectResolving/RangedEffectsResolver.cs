using SharpBotz.Arenas;
using SharpBotz.Botz.BotModules.RangedWeapons;

namespace SharpBotz.Worlds.EffectResolving;

public static class RangedEffectsResolver
{
    public static void Handle(Arena arena, BotStateEffect[] botStateEffects)
    {
        var occupants = botStateEffects.ToLookup(botStateEffect =>
            botStateEffect.BotState.Position.ToCoordinates());

        foreach (var attacker in botStateEffects)
        {
            foreach (var effect in attacker.Effects.RangedEffects.OfType<RangedEffect>())
            {
                Fire(arena, occupants, attacker, effect);
            }
            foreach (var effect in attacker.Effects.RangedEffects.OfType<RangedOverChargedEffect>())
            {
                attacker.BotState.Bot.TakeDamage(effect.ExcessPower * 3);
            }
        }
    }

    private static void Fire(
        Arena arena,
        ILookup<(int X, int Y), BotStateEffect> occupants,
        BotStateEffect attacker,
        RangedEffect effect)
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
                receiver.BotState.Bot.TakeDamage(effect.Damage);
            }
            return;
        }
    }
}
