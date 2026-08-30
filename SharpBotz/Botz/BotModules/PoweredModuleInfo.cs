namespace SharpBotz.Botz.BotModules;


public abstract record PoweredModuleInfo(
    ModuleId Id,
    int Weight,
    int ActivationPower,
    int MaximumPower,
    int CurrentPower)
    : ModuleInfo(Id, Weight)
{
    public int MaximumLevel => MaximumPower / ActivationPower;

    public int CurrentLevel => CurrentPower / ActivationPower;

    protected PowerAllocation Allocate(int power) => new(Id, power);

    public PowerAllocation Activate() => Allocate(ActivationPower);

    public PowerAllocation Activate(int level)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(level);
        return Allocate(level * ActivationPower);
    }
}
