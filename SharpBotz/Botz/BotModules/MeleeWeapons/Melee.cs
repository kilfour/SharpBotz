namespace SharpBotz.Botz.BotModules.MeleeWeapons;


public class Melee(
    ModuleId id,
    int damagePerPower,
    int maximumPower) : PoweredModule(id, GetWeight(damagePerPower, maximumPower))
{
    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new MeleeInfo(Id, damagePerPower, maximumPower);

    private static int GetWeight(int damagePerPower, int maximumPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damagePerPower);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumPower);

        var powerWeight = checked(maximumPower * (maximumPower + 1) / 2);
        var scaledDamageSquared = checked((long)damagePerPower * damagePerPower);
        var damageWeight = (scaledDamageSquared / 100) + (scaledDamageSquared % 100 == 0 ? 0 : 1);
        return checked(2 + powerWeight + (int)damageWeight);
    }

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
    {
        if (power > maximumPower)
        {
            yield return new MeleeOverChargedEffect(Id, power - maximumPower);
        }
        yield return new MeleeEffect(Id, power * damagePerPower);
    }
}
