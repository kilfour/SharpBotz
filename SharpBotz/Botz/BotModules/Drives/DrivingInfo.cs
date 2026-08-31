namespace SharpBotz.Botz.BotModules.Drives;


public record DrivingInfo : PoweredModuleInfo
{

    public int ThrustPerPower { get; }
    public int MaximumPower { get; }
    private readonly int loadedWeight;

    public DrivingInfo(
        ModuleId id,
        int thrustPerPower,
        int maximumPower,
        int loadedWeight)
    : base(id)
    {
        ThrustPerPower = thrustPerPower;
        MaximumPower = maximumPower;
        this.loadedWeight = loadedWeight;
    }

    public PowerAllocation Move(int speed)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(speed);
        var requiredPower = DivideRoundingUp(speed * loadedWeight, ThrustPerPower);
        return Allocate(requiredPower);
    }

    private static int DivideRoundingUp(int value, int divisor) =>
        (value / divisor) + (value % divisor == 0 ? 0 : 1);
}
