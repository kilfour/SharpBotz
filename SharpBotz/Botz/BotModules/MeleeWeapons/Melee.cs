using SharpBotz.Maths;

namespace SharpBotz.Botz.BotModules.MeleeWeapons;


public sealed class Melee : PoweredModule
{
    private const int StandardDamagePerPower = 4;

    private readonly int damagePerPower;
    private readonly int maximumPower;

    public Melee(
        ModuleId id,
        int damagePerPower,
        int maximumPower)
        : base(id, GetWeight(damagePerPower, maximumPower))
    {
        this.damagePerPower = damagePerPower;
        this.maximumPower = maximumPower;
    }

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new MeleeInfo(Id, damagePerPower, maximumPower);

    private static int GetWeight(int damage, int activationPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(damage);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activationPower);

        var scaledDamageSquared = checked((long)damage * damage);
        var damageWeight = (scaledDamageSquared / 100) +
                           (scaledDamageSquared % 100 == 0 ? 0 : 1);
        var standardPower = Divide.RoundingUp(damage, StandardDamagePerPower);
        var relativeEfficiency = Divide.RoundingUp(standardPower, activationPower);
        var efficiencyWeight = Math.Max(0, relativeEfficiency - 1);
        return checked(2 + (int)damageWeight + efficiencyWeight);
    }

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
    {
        if (power > maximumPower)
        {
            yield return new MeleeOverChargedEffect(Id);
        }
        yield return new MeleeEffect(Id, power * damagePerPower);
    }
}
