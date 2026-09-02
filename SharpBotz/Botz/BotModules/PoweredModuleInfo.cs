namespace SharpBotz.Botz.BotModules;


public abstract record PoweredModuleInfo(ModuleId Id) : ModuleInfo(Id)
{
    protected PowerAllocation Allocate(int power) => new(Id, power);
}
