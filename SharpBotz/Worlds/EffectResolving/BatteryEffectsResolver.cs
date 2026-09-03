using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;

namespace SharpBotz.Worlds.EffectResolving;

public static class BatteryEffectsResolver
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
        var reactorEffects = botStateEffect.Effects.BatteryEffects;
        foreach (var effect in reactorEffects)
        {
            switch (effect)
            {
                case BatteryDrainedEffect batteryEffect:
                    bot.TakeDamage(batteryEffect.ExcessPower * 2);
                    break;

                case BatteryOverChargedEffect batteryEffect:
                    bot.TakeDamage(batteryEffect.ExcessPower * 5);
                    break;

                case PowerCannotBeStoredEffect batteryEffect:
                    bot.TakeDamage(batteryEffect.ExcessPower * 10);
                    break;

                default:
                    throw new ArgumentException("Unknown battery effect supplied.");
            }
        }
    }
}
