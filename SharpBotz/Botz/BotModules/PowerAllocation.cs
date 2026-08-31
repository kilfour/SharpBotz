namespace SharpBotz.Botz.BotModules;

public record PowerAllocation(ModuleId ModuleId, int Power) : PowerModuleIntent(ModuleId);

public class PowerPlan
{
    public PowerPlan(params PowerModuleIntent[] intentions)
    {
        ArgumentNullException.ThrowIfNull(intentions);
        Generations = Array.AsReadOnly([.. intentions.Where(a => a is PowerGeneration).Cast<PowerGeneration>()]);
        Allocations = Array.AsReadOnly([.. intentions.Where(a => a is PowerAllocation).Cast<PowerAllocation>()]);
    }

    public IReadOnlyList<PowerGeneration> Generations { get; }

    public IReadOnlyList<PowerAllocation> Allocations { get; }

    public static PowerPlan Empty { get; } = new();
}