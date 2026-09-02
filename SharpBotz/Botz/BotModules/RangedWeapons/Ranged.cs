using SharpBotz.Maths;

namespace SharpBotz.Botz.BotModules.RangedWeapons;

public class Ranged(
    ModuleId id,
    int range,
    int damagePerPower,
    int maximumPower) : PoweredModule(
        id,
        GetWeight(range, damagePerPower, maximumPower))
{
    private const int StandardDamagePerPower = 2;

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new RangedInfo(Id, range, damagePerPower, maximumPower);

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
    {
        if (power > maximumPower)
        {
            yield return new RangedOverChargedEffect(Id);
        }
        yield return new RangedEffect(Id, range, power * damagePerPower);
    }

    private static int GetWeight(int range, int damage, int maximumPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(range);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damage);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumPower);

        var rangeWeight = checked(range * (range + 1) / 2);
        var scaledDamageSquared = checked((long)damage * damage);
        var damageWeight = (scaledDamageSquared / 100) +
                           (scaledDamageSquared % 100 == 0 ? 0 : 1);
        var standardPower = Divide.RoundingUp(damage, StandardDamagePerPower);
        var relativeEfficiency = Divide.RoundingUp(standardPower, maximumPower);
        var efficiencyWeight = Math.Max(0, relativeEfficiency - 1);
        return checked(3 + rangeWeight + (int)damageWeight + efficiencyWeight);
    }
}
