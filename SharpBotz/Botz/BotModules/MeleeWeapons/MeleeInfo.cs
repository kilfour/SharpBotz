using SharpBotz.Maths;

namespace SharpBotz.Botz.BotModules.MeleeWeapons;

public record MeleeInfo(
    ModuleId Id,
    int DamagePerPower,
    int MaximumPower)
    : PoweredModuleInfo(Id)
{
    public PowerAllocation Hit(int damage)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damage);
        var requiredPower = Divide.RoundingUp(damage, DamagePerPower);
        return Allocate(requiredPower);
    }
}
