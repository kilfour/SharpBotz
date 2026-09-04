namespace SharpBotz.Botz.BotModules;

public record PowerAllocation(ModuleId ModuleId, int Power) : PowerModuleIntent(ModuleId);

public class PowerPlan
{
    private PowerPlan(params PowerModuleIntent[] intentions)
    {
        ArgumentNullException.ThrowIfNull(intentions);
        if (intentions.Select(intention => intention.ModuleId).Distinct().Count() != intentions.Length)
        {
            throw new ArgumentException(
                "A module can only be activated once per power plan.",
                nameof(intentions));
        }

        Generations = Array.AsReadOnly([.. intentions.Where(a => a is PowerGeneration).Cast<PowerGeneration>()]);
        Allocations = Array.AsReadOnly([.. intentions.Where(a => a is PowerAllocation).Cast<PowerAllocation>()]);
    }

    public static PowerPlan From(params PowerModuleIntent[] intentions) =>
        new(intentions);

    public IReadOnlyList<PowerGeneration> Generations { get; }

    public IReadOnlyList<PowerAllocation> Allocations { get; }

    public static PowerPlan Empty { get; } = new();
}
