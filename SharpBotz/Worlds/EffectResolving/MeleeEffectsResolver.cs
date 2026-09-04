using SharpBotz.Botz.BotModules.MeleeWeapons;

namespace SharpBotz.Worlds.EffectResolving;

public static class MeleeEffectsResolver
{
    public static void Handle(BotStateEffect[] botStateEffects)
    {
        var occupants = botStateEffects.ToLookup(botStateEffect =>
            botStateEffect.BotState.Position.ToCoordinates());

        foreach (var attacker in botStateEffects)
        {
            if (!attacker.BotState.Bot.IsAlive)
                continue;

            var attackerState = attacker.BotState;
            var target = attackerState.Position.Move(attackerState.Facing);

            foreach (var effect in attacker.Effects.MeleeEffects.OfType<MeleeEffect>())
            {
                var receivers = occupants[target.ToCoordinates()]
                    .Where(receiver =>
                        receiver.BotState.Bot.IsAlive &&
                        !ReferenceEquals(attackerState.Bot, receiver.BotState.Bot));

                foreach (var receiver in receivers)
                {
                    receiver.BotState.Bot.TakeDamage(effect.Damage);
                }
            }
            foreach (var effect in attacker.Effects.MeleeEffects.OfType<MeleeOverChargedEffect>())
            {
                attacker.BotState.Bot.TakeDamage(effect.ExcessPower * 3);
            }
        }
    }
}
