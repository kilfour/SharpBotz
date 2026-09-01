using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Worlds.EffectResolving;

public static class ReactorEffectsResolver
{
    public static void Handle(BotStateEffect[] botStateEffects)
    {
        foreach (var botStateEffect in botStateEffects)
        {
            HandleReactorEffect(botStateEffect);
        }
    }

    private static void HandleReactorEffect(BotStateEffect botStateEffect)
    {
        var bot = botStateEffect.BotState.Bot;
        var reactorEffects = botStateEffect.Effects.ReactorEffects;
        foreach (var effect in reactorEffects)
        {
            switch (effect)
            {
                case ReactorOverLoadedEffect:
                    bot.TakeDamage(10);
                    break;

                default:
                    throw new ArgumentException("Unknown reactor effect supplied.");
            }
        }
    }
}