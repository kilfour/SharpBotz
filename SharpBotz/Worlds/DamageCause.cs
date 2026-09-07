using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Worlds;

public abstract record DamageCause
{
    public sealed record Collision : DamageCause;

    public sealed record ReactorOverload(ModuleId ModuleId) : DamageCause;

    public sealed record BatteryDrained(ModuleId ModuleId) : DamageCause;

    public sealed record BatteryOvercharged(ModuleId ModuleId) : DamageCause;

    public sealed record PowerNotStored(ModuleId ModuleId) : DamageCause;

    public sealed record ThrusterOvercharged(ModuleId ModuleId) : DamageCause;

    public sealed record RotatorOvercharged(ModuleId ModuleId) : DamageCause;

    public sealed record MeleeAttack(Bot Attacker, ModuleId ModuleId) : DamageCause;

    public sealed record MeleeOvercharged(ModuleId ModuleId) : DamageCause;

    public sealed record RangedAttack(Bot Attacker, ModuleId ModuleId) : DamageCause;

    public sealed record RangedOvercharged(ModuleId ModuleId) : DamageCause;

    public sealed record ScannerOvercharged(ModuleId ModuleId) : DamageCause;
}
