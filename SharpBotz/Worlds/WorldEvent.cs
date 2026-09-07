using SharpBotz.Botz;

namespace SharpBotz.Worlds;

public abstract record WorldEvent;

public record BotDamaged(
    Bot Bot,
    int Damage,
    DamageCause Cause) : WorldEvent;
