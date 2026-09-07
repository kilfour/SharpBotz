namespace SharpBotz.Botz.BotModules.Thrusters;

public class Thruster : PoweredModule
{
    private readonly int thrustPerPower;
    private readonly int maximumPower;

    private Thruster(
        ModuleId id,
        int thrustPerPower,
        int maximumPower)
        : base(id, GetWeight(thrustPerPower, maximumPower))
    {
        this.thrustPerPower = thrustPerPower;
        this.maximumPower = maximumPower;
    }

    public static ThrusterThrustPerPower Named(string moduleId) =>
        new(ModuleId.Is(moduleId));

    public class ThrusterThrustPerPower(ModuleId id)
    {
        public ThrusterMaximumPower ThrustPerPower(int thrustPerPower) =>
            new(id, thrustPerPower);
    }

    public class ThrusterMaximumPower(ModuleId id, int thrustPerPower)
    {
        public Thruster MaximumPower(int maximumPower) =>
            new(id, thrustPerPower, maximumPower);
    }

    protected override ModuleInfo CreateInfo(int totalWeight) =>
        new ThrusterInfo(Id, thrustPerPower, maximumPower, totalWeight);

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
    {
        if (power > maximumPower)
        {
            yield return new ThrusterOverChargedEffect(Id, power - maximumPower);
        }
        yield return new ThrusterEffect(Id, power * thrustPerPower / totalBotWeight);
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
