using SharpBotz.Maths;

namespace SharpBotz.Botz.BotModules.RangedWeapons;

public record RangedInfo(
    ModuleId Id,
    int Range,
    int DamagePerPower,
    int MaximumPower)
    : PoweredModuleInfo(Id)
{
    public PowerAllocation Fire(int damage)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damage);
        var requiredPower = Divide.RoundingUp(damage, DamagePerPower);
        return Allocate(requiredPower);
    }
}
