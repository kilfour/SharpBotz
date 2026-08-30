namespace SharpBotz.Botz.BotModules.Drives;

public class Drive : PoweredModule
{
    private readonly int maximumSpeed;
    private readonly int thrustPerPower;

    private Drive(
        ModuleId id,
        int activationPower,
        int maximumSpeed,
        int thrustPerPower)
        : base(
            id,
            GetWeight(maximumSpeed, thrustPerPower),
            activationPower,
            GetMaximumPower(activationPower, maximumSpeed))
    {
        this.maximumSpeed = maximumSpeed;
        this.thrustPerPower = thrustPerPower;
    }

    public static Drive Create(
        string moduleId,
        int activationPower,
        int maximumSpeed,
        int thrustPerPower)
        => new(ModuleId.Is(moduleId), activationPower, maximumSpeed, thrustPerPower);

    public int CurrentThrust => CurrentPower * thrustPerPower;

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new DriveInfo(
            Id,
            Weight,
            ActivationPower,
            MaximumPower,
            CurrentPower,
            maximumSpeed,
            GetSpeed(MaximumPower, totalWeight),
            GetSpeed(CurrentPower, totalWeight),
            thrustPerPower,
            CurrentThrust,
            totalWeight);

    protected override IEnumerable<ModuleEffect> CreateEffects()
    {
        yield return new ThrustEffect(Id, CurrentThrust, maximumSpeed);
    }

    private static int GetMaximumPower(int activationPower, int maximumSpeed)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumSpeed);
        return activationPower * maximumSpeed;
    }

    private int GetSpeed(int power, int totalWeight) =>
        Math.Min(maximumSpeed, power * thrustPerPower / totalWeight);

    private static int GetWeight(int maximumSpeed, int thrustPerPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumSpeed);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(thrustPerPower);

        var speedWeight = maximumSpeed * (maximumSpeed + 1) / 2;
        var efficiencyWeight = Math.Max(0, thrustPerPower - 10);
        efficiencyWeight = (efficiencyWeight / 2) + (efficiencyWeight % 2);
        return 3 + speedWeight + efficiencyWeight;
    }
}
