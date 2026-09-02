namespace SharpBotz.Botz.BotModules.Scanners;


public sealed class Scanner(ModuleId id, int powerPerRange, int maximumPower)
    : PoweredModule(id, GetWeight(powerPerRange))
{
    private const int StandardPowerPerRange = 3;

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new ScannerInfo(Id, powerPerRange, maximumPower);

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
    {
        if (power > maximumPower)
        {
            yield return new ScannerOverChargedEffect(Id);
        }
        yield return new ScanEffect(Id, power / powerPerRange);
    }

    private static int GetWeight(int powerPerRange)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(powerPerRange);

        var efficiencyWeight = Math.Max(0, StandardPowerPerRange - powerPerRange);
        return checked(2 + efficiencyWeight);
    }
}
