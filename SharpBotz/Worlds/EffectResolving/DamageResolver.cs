using SharpBotz.Botz;

namespace SharpBotz.Worlds.EffectResolving;

internal static class DamageResolver
{
    public static void Handle(
        Bot bot,
        int damage,
        DamageCause cause,
        ICollection<WorldEvent> worldEvents)
    {
        var hitPointsBeforeDamage = bot.HitPoints;
        bot.TakeDamage(damage);
        var damageTaken = hitPointsBeforeDamage - bot.HitPoints;

        if (damageTaken > 0)
        {
            worldEvents.Add(new BotDamaged(bot, damageTaken, cause));
        }
    }
}
