using SharpBotz.Botz.BotModules;
using SharpBotz.Botz.BotModules.Batteries;

namespace SharpBotz.Worlds.EffectResolving;

public static class BatteryEffectsResolver
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
        var reactorEffects = botStateEffect.Effects.BatteryEffects;
        foreach (var effect in reactorEffects)
        {
            switch (effect)
            {
                case BatteryDrainedEffect batteryEffect:
                    DamageResolver.Handle(
                        bot,
                        batteryEffect.ExcessPower * 2,
                        new DamageCause.BatteryDrained(batteryEffect.Id),
                        worldEvents);
                    break;

                case BatteryOverChargedEffect batteryEffect:
                    DamageResolver.Handle(
                        bot,
                        batteryEffect.ExcessPower * 5,
                        new DamageCause.BatteryOvercharged(batteryEffect.Id),
                        worldEvents);
                    break;

                case PowerCannotBeStoredEffect batteryEffect:
                    DamageResolver.Handle(
                        bot,
                        batteryEffect.ExcessPower * 10,
                        new DamageCause.PowerNotStored(batteryEffect.Id),
                        worldEvents);
                    break;

                default:
                    throw new ArgumentException("Unknown battery effect supplied.");
            }
        }
    }
}
