namespace SharpBotz.Botz.BotModules.RangedWeapons;

public class Ranged(
    ModuleId id,
    int range,
    int damagePerPower,
    int maximumPower) : PoweredModule(
        id,
        GetWeight(range, damagePerPower, maximumPower))
{
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

    private static int GetWeight(int range, int damagePerPower, int maximumPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(range);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damagePerPower);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumPower);

        var rangeWeight = checked(range * (range + 1) / 2);
        var powerWeight = checked(maximumPower * (maximumPower + 1) / 2);
        var scaledDamageSquared = checked((long)damagePerPower * damagePerPower);
        var damageWeight = (scaledDamageSquared / 100) +
                           (scaledDamageSquared % 100 == 0 ? 0 : 1);
        return checked(3 + rangeWeight + powerWeight + (int)damageWeight);
    }
}
