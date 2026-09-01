

namespace SharpBotz.Botz.BotModules.Rotators;

public class Rotator : PoweredModule
{
    private const int StandardActivationPower = 5;

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
        return rotation == Rotation.Left
            ? new LeftRotatorInfo(Id)
            : new RightRotatorInfo(Id);
    }

    public override IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight)
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
}
