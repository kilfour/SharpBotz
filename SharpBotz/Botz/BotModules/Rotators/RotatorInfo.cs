using SharpBotz.Maths;

namespace SharpBotz.Botz.BotModules.Rotators;


public abstract record RotatorInfo : PoweredModuleInfo
{
    public int TorquePerPower { get; }
    public int MaximumPower { get; }
    private int loadedWeight;

    protected RotatorInfo(
        ModuleId id,
        int torquePerPower,
        int maximumPower,
        int loadedWeight) : base(id)
    {
        TorquePerPower = torquePerPower;
        MaximumPower = maximumPower;
        this.loadedWeight = loadedWeight;
    }

    public PowerAllocation Turn(int times)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(times);
        var requiredPower = Divide.RoundingUp(times * loadedWeight, TorquePerPower);
        return Allocate(requiredPower);
    }
}

public record LeftRotatorInfo : RotatorInfo
{
    public LeftRotatorInfo(
        ModuleId id,
        int torquePerPower,
        int maximumPower,
        int loadedWeight) : base(id, torquePerPower, maximumPower, loadedWeight) { }
}

public record RightRotatorInfo : RotatorInfo
{
    public RightRotatorInfo(
        ModuleId id,
        int torquePerPower,
        int maximumPower,
        int loadedWeight) : base(id, torquePerPower, maximumPower, loadedWeight) { }
}