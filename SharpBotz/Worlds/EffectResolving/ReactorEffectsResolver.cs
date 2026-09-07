using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Worlds.EffectResolving;


public static class ReactorEffectsResolver
{
    public static void Handle(
        BotStateEffect[] botStateEffects,
        ICollection<WorldEvent> worldEvents)
    {
        foreach (var botStateEffect in botStateEffects)
        {
            HandleEffect(botStateEffect, worldEvents);
        }
    }

    private static void HandleEffect(
        BotStateEffect botStateEffect,
        ICollection<WorldEvent> worldEvents)
    {
        var bot = botStateEffect.BotState.Bot;
        var reactorEffects = botStateEffect.Effects.ReactorEffects;
        foreach (var effect in reactorEffects)
        {
            switch (effect)
            {
                case ReactorOverLoadedEffect overload:
                    DamageResolver.Handle(
                        bot,
                        overload.ExcessPower * 2,
                        new DamageCause.ReactorOverload(overload.Id),
                        worldEvents);
                    break;

                default:
                    throw new ArgumentException("Unknown reactor effect supplied.");
            }
        }
    }
}
