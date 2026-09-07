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

        MeleeEffectsResolver.HandleParticipants(participants, worldEvents);
        RangedEffectsResolver.HandleParticipants(arena, participants, worldEvents);
    }
}
