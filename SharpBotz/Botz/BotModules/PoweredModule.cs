namespace SharpBotz.Botz.BotModules;


public abstract class PoweredModule(ModuleId id, int weight) : BotModule(id, weight)
{
    public IEnumerable<ModuleEffect> Supply(int power, int totalBotWeight) =>
        CreateEffects(power, totalBotWeight);

    public abstract IEnumerable<ModuleEffect> CreateEffects(int power, int totalBotWeight);
}
