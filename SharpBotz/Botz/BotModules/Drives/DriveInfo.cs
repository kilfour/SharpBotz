namespace SharpBotz.Botz.BotModules.Drives;


public record DriveInfo(
    ModuleId Id,
    int Weight,
    int ActivationPower,
    int MaximumPower,
    int CurrentPower,
    int RatedMaximumSpeed,
    int MaximumSpeed,
    int CurrentSpeed,
    int ThrustPerPower,
    int CurrentThrust,
    int LoadedWeight)
    : PoweredModuleInfo(Id, Weight, ActivationPower, MaximumPower, CurrentPower)
{
    public int PowerRequiredForSpeed(int speed)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(speed);

        if (speed > MaximumSpeed)
        {
            throw new ArgumentOutOfRangeException(
                nameof(speed),
                speed,
                $"This drive can reach speed {MaximumSpeed} with the current load.");
        }

        var requiredPower = DivideRoundingUp(
            (long)speed * LoadedWeight,
            ThrustPerPower);
        return Math.Max(ActivationPower, (int)requiredPower);
    }

    public PowerAllocation Move(int speed) =>
        Allocate(PowerRequiredForSpeed(speed));

    private static long DivideRoundingUp(long value, int divisor) =>
        (value / divisor) + (value % divisor == 0 ? 0 : 1);
}
