namespace SharpBotz.Botz.BotModules;


public abstract class PoweredModule : BotModule
{
    protected PoweredModule(
        ModuleId id,
        int weight,
        int activationPower,
        int maximumPower)
        : base(id, weight)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(activationPower);
        ArgumentOutOfRangeException.ThrowIfLessThan(maximumPower, activationPower);

        ActivationPower = activationPower;
        MaximumPower = maximumPower;
    }

    public int ActivationPower { get; }

    public int MaximumPower { get; }

    public int CurrentPower { get; private set; }

    public void Supply(int power) => CurrentPower = power;

    public void Disconnect() => CurrentPower = 0;

    public int GetLoadedActivationPower(int totalWeight) =>
        CalculateActivationPower(totalWeight);

    public int GetLoadedMaximumPower(int totalWeight) =>
        CalculateMaximumPower(totalWeight);

    public IEnumerable<ModuleEffect> GetEffects(int totalWeight) =>
        CurrentPower >= GetLoadedActivationPower(totalWeight)
            ? CreateEffects()
            : [];

    protected virtual int CalculateActivationPower(int totalWeight) =>
        ActivationPower;

    protected virtual int CalculateMaximumPower(int totalWeight) =>
        MaximumPower;

    protected abstract IEnumerable<ModuleEffect> CreateEffects();
}
