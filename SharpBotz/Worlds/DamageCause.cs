using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Worlds;

public abstract record DamageCause
{
    public record Combined(
        IReadOnlyList<DamageContribution> Contributions) : DamageCause;

    public record Collision : DamageCause;

    public record ReactorOverload(ModuleId ModuleId) : DamageCause;

    public record BatteryDrained(ModuleId ModuleId) : DamageCause;

    public record BatteryOvercharged(ModuleId ModuleId) : DamageCause;

    public record PowerNotStored(ModuleId ModuleId) : DamageCause;

    public record ThrusterOvercharged(ModuleId ModuleId) : DamageCause;

    public record RotatorOvercharged(ModuleId ModuleId) : DamageCause;

    public record MeleeAttack(Bot Attacker, ModuleId ModuleId) : DamageCause;

    public record MeleeOvercharged(ModuleId ModuleId) : DamageCause;

    public record RangedAttack(Bot Attacker, ModuleId ModuleId) : DamageCause;

    public record RangedOvercharged(ModuleId ModuleId) : DamageCause;

    public record ScannerOvercharged(ModuleId ModuleId) : DamageCause;
}

public record DamageContribution(int Damage, DamageCause Cause);
