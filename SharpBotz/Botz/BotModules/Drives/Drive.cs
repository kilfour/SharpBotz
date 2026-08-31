namespace SharpBotz.Botz.BotModules.Drives;

public class Drive : PoweredModule
{
    private readonly int thrustPerPower;
    private readonly int maximumPower;

    private Drive(
        ModuleId id,
        int thrustPerPower,
        int maximumPower)
        : base(id, GetWeight(thrustPerPower, maximumPower))
    {
        this.thrustPerPower = thrustPerPower;
        this.maximumPower = maximumPower;
    }

    public static DriveThrustPerPower Named(string moduleId) =>
        new(ModuleId.Is(moduleId));

    public class DriveThrustPerPower(ModuleId id)
    {
        public DriveMaximumPower ThrustPerPower(int thrustPerPower) =>
            new(id, thrustPerPower);
    }

    public class DriveMaximumPower(ModuleId id, int thrustPerPower)
    {
        public Drive MaximumPower(int maximumPower) =>
            new(id, thrustPerPower, maximumPower);
    }

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new DrivingInfo(Id, thrustPerPower, maximumPower, totalWeight);

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
    {
        if (power > maximumPower)
        {
            yield return new DriveOverChargedEffect(Id);
            yield break;
        }
        yield return new ThrustEffect(Id, power * thrustPerPower / totalBotWeight);
    }

    private static int GetWeight(int thrustPerPower, int maximumPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumPower);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(thrustPerPower);

        var speedWeight = maximumPower * (maximumPower + 1) / 2;
        var efficiencyWeight = Math.Max(0, thrustPerPower - 10);
        efficiencyWeight = (efficiencyWeight / 2) + (efficiencyWeight % 2);
        return 3 + speedWeight + efficiencyWeight;
    }
}
