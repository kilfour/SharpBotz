using SharpBotz.Botz.BotModules.MeleeWeapons;

namespace SharpBotz.Worlds.EffectResolving;

public static class MeleeEffectsResolver
{
    public static void Handle(
        BotStateEffect[] botStateEffects,
        ICollection<WorldEvent> worldEvents)
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
                    DamageResolver.Handle(
                        receiver.BotState.Bot,
                        effect.Damage,
                        new DamageCause.MeleeAttack(attackerState.Bot, effect.Id),
                        worldEvents);
                }
            }
            foreach (var effect in attacker.Effects.MeleeEffects.OfType<MeleeOverChargedEffect>())
            {
                DamageResolver.Handle(
                    attacker.BotState.Bot,
                    effect.ExcessPower * 3,
                    new DamageCause.MeleeOvercharged(effect.Id),
                    worldEvents);
            }
        }
    }
}
