using SharpBotz.Botz;

namespace SharpBotz.Worlds.EffectResolving;

public record PendingDamage(
    Bot Bot,
    int Damage,
    DamageCause Cause);

public static class DamageResolver
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

    public static void Handle(
        IEnumerable<PendingDamage> pendingDamage,
        ICollection<WorldEvent> worldEvents)
    {
        var damageByBot = pendingDamage
            .GroupBy(pending => pending.Bot)
            .OrderBy(group => group.Key.Name, StringComparer.Ordinal);

        foreach (var damageGroup in damageByBot)
        {
            var damage = damageGroup
                .OrderBy(pending => GetCauseSortKey(pending.Cause), StringComparer.Ordinal)
                .ThenBy(pending => pending.Damage)
                .ToArray();
            var requestedDamage = damage.Sum(pending => (long)pending.Damage);
            var cause = damage.Length == 1
                ? damage[0].Cause
                : new DamageCause.Combined(
                    Array.AsReadOnly(
                        damage
                            .Select(pending => new DamageContribution(
                                pending.Damage,
                                pending.Cause))
                            .ToArray()));

            Handle(
                damageGroup.Key,
                (int)Math.Min(requestedDamage, int.MaxValue),
                cause,
                worldEvents);
        }
    }

    private static string GetCauseSortKey(DamageCause cause) =>
        cause switch
        {
            DamageCause.MeleeAttack attack =>
                $"MeleeAttack|{attack.Attacker.Name}|{attack.ModuleId}",
            DamageCause.RangedAttack attack =>
                $"RangedAttack|{attack.Attacker.Name}|{attack.ModuleId}",
            DamageCause.MeleeOvercharged overcharged =>
                $"MeleeOvercharged|{overcharged.ModuleId}",
            DamageCause.RangedOvercharged overcharged =>
                $"RangedOvercharged|{overcharged.ModuleId}",
            _ => cause.GetType().Name,
        };
}
