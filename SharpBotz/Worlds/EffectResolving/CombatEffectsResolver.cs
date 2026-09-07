using SharpBotz.Arenas;

namespace SharpBotz.Worlds.EffectResolving;

public static class CombatEffectsResolver
{
    public static void Handle(
        Arena arena,
        BotStateEffect[] botStateEffects,
        ICollection<WorldEvent> worldEvents)
    {
        var participants = botStateEffects
            .Where(botStateEffect => botStateEffect.BotState.Bot.IsAlive)
            .ToArray();
        var pendingDamage = new List<PendingDamage>();

        MeleeEffectsResolver.HandleParticipants(participants, pendingDamage);
        RangedEffectsResolver.HandleParticipants(arena, participants, pendingDamage);
        DamageResolver.Handle(pendingDamage, worldEvents);
    }
}
