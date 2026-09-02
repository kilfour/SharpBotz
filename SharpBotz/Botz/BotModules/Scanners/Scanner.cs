namespace SharpBotz.Botz.BotModules.Scanners;


public class Scanner : PoweredModule
{
    private const int StandardPowerPerRange = 3;
    private readonly int powerPerRange;
    private readonly int maximumPower;

    private Scanner(
        ModuleId id,
        int powerPerRange,
        int maximumPower)
        : base(id, GetWeight(powerPerRange, maximumPower))
    {
        this.powerPerRange = powerPerRange;
        this.maximumPower = maximumPower;
    }

    public static ScannerPowerPerRange Named(string moduleId) =>
        new(ModuleId.Is(moduleId));

    public class ScannerPowerPerRange(ModuleId id)
    {
        public ScannerMaximumPower PowerPerRange(int powerPerRange) =>
            new(id, powerPerRange);
    }

    public class ScannerMaximumPower(ModuleId id, int powerPerRange)
    {
        public Scanner MaximumPower(int maximumPower) =>
            new(id, powerPerRange, maximumPower);
    }

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
