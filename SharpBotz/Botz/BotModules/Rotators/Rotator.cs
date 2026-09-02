

namespace SharpBotz.Botz.BotModules.Rotators;

public class Rotator : PoweredModule
{
    private readonly int torquePerPower;
    private readonly int maximumPower;
    private readonly Rotation rotation;

    private Rotator(
        ModuleId id,
        int torquePerPower,
        int maximumPower,
        Rotation rotation)
        : base(id, GetWeight(torquePerPower, maximumPower))
    {
        this.torquePerPower = torquePerPower;
        this.maximumPower = maximumPower;
        this.rotation = rotation;
    }

    public static RotatorTorquePerPower Named(string moduleId) =>
        new(ModuleId.Is(moduleId));

    public class RotatorTorquePerPower(ModuleId id)
    {
        public RotatorMaximumPower TorquePerPower(int torquePerPower)
            => new(id, torquePerPower);
    }

    public class RotatorMaximumPower(ModuleId id, int torquePerPower)
    {
        public RotatorDirection MaximumPower(int maximumPower)
            => new(id, torquePerPower, maximumPower);
    }

    public class RotatorDirection(ModuleId id, int torquePerPower, int maximumPower)
    {
        public Rotator Left() => new(id, torquePerPower, maximumPower, Rotation.Left);
        public Rotator Right() => new(id, torquePerPower, maximumPower, Rotation.Right);
    }

    protected override ModuleInfo CreateInfo(int totalWeight)
    {
        return rotation == Rotation.Left
            ? new LeftRotatorInfo(Id)
            : new RightRotatorInfo(Id);
    }

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
    {
        if (power > maximumPower)
        {
            yield return new RotatorOverChargedEffect(Id, power - maximumPower);
        }
        yield return new RotateEffect(Id, rotation, power * torquePerPower / totalBotWeight);
    }

    private static int GetWeight(int torquePerPower, int maximumPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maximumPower);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(torquePerPower);

        var speedWeight = maximumPower * (maximumPower + 1) / 2;
        var efficiencyWeight = Math.Max(0, torquePerPower - 10);
        efficiencyWeight = (efficiencyWeight / 2) + (efficiencyWeight % 2);
        return 3 + speedWeight + efficiencyWeight;
    }
}
