using SharpBotz.Maths;

namespace SharpBotz.Botz.BotModules.Thrusters;


public record ThrusterInfo : PoweredModuleInfo
{

    public int ThrustPerPower { get; }
    public int MaximumPower { get; }
    private readonly int loadedWeight;

    public ThrusterInfo(
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
        var requiredPower = Divide.RoundingUp(speed * loadedWeight, ThrustPerPower);
        return Allocate(requiredPower);
    }
}
