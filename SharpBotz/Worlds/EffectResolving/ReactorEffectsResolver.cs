using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Worlds.EffectResolving;


public static class ReactorEffectsResolver
{
    public static void Handle(BotStateEffect[] botStateEffects)
    {
        foreach (var botStateEffect in botStateEffects)
        {
            HandleEffect(botStateEffect);
        }
    }

    private static void HandleEffect(BotStateEffect botStateEffect)
    {
        var bot = botStateEffect.BotState.Bot;
        var reactorEffects = botStateEffect.Effects.ReactorEffects;
        foreach (var effect in reactorEffects)
        {
            switch (effect)
            {
                case ReactorOverLoadedEffect overload:
                    bot.TakeDamage(overload.ExcessPower * 2);
                    break;

                default:
                    throw new ArgumentException("Unknown reactor effect supplied.");
            }
        }
    }
}
