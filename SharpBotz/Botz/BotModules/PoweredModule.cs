namespace SharpBotz.Botz.BotModules;


public abstract class PoweredModule : BotModule
{
    protected PoweredModule(
        ModuleId id,
        int weight)
        : base(id, weight)
    {
        //MaximumPower = maximumPower;
    }

    //public int MaximumPower { get; }

    public IEnumerable<ModuleEffect> Supply(int power, int totalBotWeight) => CreateEffects(power, totalBotWeight);

    // public void Disconnect() => CurrentPower = 0;


    public abstract IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight);
}
