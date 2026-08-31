namespace SharpBotz.Botz.BotModules;


public abstract class PoweredModule : BotModule
{
    protected PoweredModule(
        ModuleId id,
        int weight/*,
        int maximumPower*/)
        : base(id, weight)
    {
        //MaximumPower = maximumPower;
    }

    //public int MaximumPower { get; }

    public int CurrentPower { get; private set; }

    public void Supply(int power) => CurrentPower = power;

    public void Disconnect() => CurrentPower = 0;

    // public int GetLoadedActivationPower(int totalWeight) =>
    //     CalculateActivationPower(totalWeight);

    // public int GetLoadedMaximumPower(int totalWeight) =>
    //     CalculateMaximumPower(totalWeight);

    public IEnumerable<ModuleEffect> GetEffects(int totalWeight) =>
         // CurrentPower >= GetLoadedActivationPower(totalWeight)
         //     ? CreateEffects()
         //     : [];
         [];

    // protected virtual int CalculateActivationPower(int totalWeight) =>
    //     ActivationPower;

    // protected virtual int CalculateMaximumPower(int totalWeight) =>
    //     MaximumPower;

    protected abstract IEnumerable<ModuleEffect> CreateEffects();
}
