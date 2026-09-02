namespace SharpBotz.Botz.BotModules.Scanners;

public record ScannerInfo(
    ModuleId Id,
    int PowerPerRange,
    int MaximumPower)
    : PoweredModuleInfo(Id)
{
    public int MaximumRange => MaximumPower / PowerPerRange;

    public PowerAllocation Scan(int range)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(range);
        return Allocate(range * PowerPerRange);
    }
}
