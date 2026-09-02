namespace SharpBotz.Botz.BotModules.Scanners;


public sealed class Scanner(ModuleId id, int powerPerRange, int maximumPower)
    : PoweredModule(id, GetWeight(powerPerRange, maximumPower))
{
    private const int StandardPowerPerRange = 3;

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new ScannerInfo(Id, powerPerRange, maximumPower);

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
    {
        if (power > maximumPower)
        {
            yield return new ScannerOverChargedEffect(Id, power - maximumPower);
        }
        yield return new ScanEffect(Id, power / powerPerRange);
    }

    private static int GetWeight(int powerPerRange, int maximumPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(powerPerRange);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumPower);

        var maximumRange = maximumPower / powerPerRange;
        var rangeWeight = checked(maximumRange * (maximumRange + 1) / 2);
        var efficiencyWeight = Math.Max(0, StandardPowerPerRange - powerPerRange);
        return checked(2 + rangeWeight + efficiencyWeight);
    }
}
