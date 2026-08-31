

namespace SharpBotz.Botz.BotModules.Rotators;

public sealed class Rotator : PoweredModule
{
    private const int StandardActivationPower = 5;
    private const int RatedBotWeight = 50;

    private readonly Rotation rotation;

    private Rotator(
        ModuleId id,
        int activationPower,
        Rotation rotation)
        : base(id, GetWeight(activationPower))
    {
        this.rotation = rotation;
    }

    public static Rotator Left(ModuleId id, int activationPower) =>
        new(id, activationPower, Rotation.Left);

    public static Rotator Right(ModuleId id, int activationPower) =>
        new(id, activationPower, Rotation.Right);

    protected override ModuleInfo CreateInfo(int totalWeight)
    {
        var requiredPower = 0;// GetLoadedActivationPower(totalWeight);
        return rotation == Rotation.Left
            ? new LeftRotatorInfo(Id, Weight, requiredPower, requiredPower, CurrentPower)
            : new RightRotatorInfo(Id, Weight, requiredPower, requiredPower, CurrentPower);
    }

    // protected override int CalculateActivationPower(int totalWeight) =>
    //     GetRequiredPower(totalWeight);

    // protected override int CalculateMaximumPower(int totalWeight) =>
    //     GetRequiredPower(totalWeight);

    protected override IEnumerable<ModuleEffect> CreateEffects()
    {
        yield return new RotateEffect(Id, rotation);
    }

    private static int GetWeight(int activationPower)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activationPower);
        var relativeEfficiency = DivideRoundingUp(
            StandardActivationPower,
            activationPower);
        return checked(1 + relativeEfficiency);
    }

    private static int DivideRoundingUp(int value, int divisor) =>
        (value / divisor) + (value % divisor == 0 ? 0 : 1);

    private int GetRequiredPower(int totalWeight)
    {
        // ArgumentOutOfRangeException.ThrowIfNegativeOrZero(totalWeight);
        // var scaledPower = DivideRoundingUp(
        //     totalWeight * ActivationPower, RatedBotWeight);
        // return Math.Max(ActivationPower, scaledPower);
        return 0;
    }
}
