using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Botz.BotModules;

public class ModuleRack
{
    public const int ChassisWeight = 10;
    private readonly BotModule[] modules;
    private readonly IReadOnlyDictionary<ModuleId, BotModule> modulesById;
    private readonly Reactor[] reactors;
    // private readonly PoweredModule[] poweredModules;
    private readonly Battery[] batteries;
    private bool isAttached;

    private ModuleRack(params BotModule[] modules)
    {
        this.modules = [.. modules];
        TotalWeight = ChassisWeight + this.modules.Sum(module => module.Weight);
        foreach (var module in this.modules)
        {
            module.Install();
        }
        modulesById = this.modules.ToDictionary(module => module.Id);
        reactors = [.. this.modules.OfType<Reactor>()];
        // poweredModules = [.. this.modules.OfType<PoweredModule>()];
        batteries = [.. this.modules.OfType<Battery>()];
    }

    public static ModuleRack Create(params BotModule[] modules)
    {
        ArgumentNullException.ThrowIfNull(modules);
        if (modules.Any(module => module is null))
            throw new ArgumentException("A module rack cannot contain null modules.", nameof(modules));

        if (modules.Distinct().Count() != modules.Length)
            throw new ArgumentException("A module instance can only be installed once.", nameof(modules));

        if (modules.Select(module => module.Id).Distinct().Count() != modules.Length)
            throw new ArgumentException("Every installed module must have a unique ID.", nameof(modules));

        if (modules.Any(module => module.IsInstalled))
            throw new ArgumentException("A module can only be installed in one rack.", nameof(modules));
        return new(modules);
    }

    public int TotalWeight { get; }
    public int BatteryLevel => batteries.Sum(battery => battery.Charge);

    // only used in tests for now
    public int BatteryCapacity => batteries.Sum(battery => battery.Capacity);
    // only used in tests for now
    public int MaximumReactorOutput => reactors.Sum(reactor => reactor.MaximumOutput);

    public ModuleControl GetModuleControl() =>
        new([.. modules.Select(module => module.GetInfo(TotalWeight))]);


    public ModuleEffect[] Resolve(PowerPlan plan)
    {
        var batteryCount = batteries.Length;
        var effects = new List<ModuleEffect>();

        var remaining = 0;
        foreach (var (id, power) in plan.Generations)
        {
            var maximumOutput = reactors.Single(a => a.Id == id).MaximumOutput;
            if (power > maximumOutput)
                effects.Add(new ReactorOverLoadedEffect(id, power - maximumOutput));
            else
                remaining += power;
        }

        foreach (var (id, power) in plan.Allocations)
        {
            var needs = power;
            if (needs <= remaining)
            {
                remaining -= needs;
                needs = 0;
            }
            else
            {
                needs -= remaining;
                remaining = 0;
            }
            if (needs > 0 && batteryCount > 0)
            {
                foreach (var (battery, drain) in DistributeEvenly(needs, batteries))
                {
                    var batteryEffect = battery.Drain(drain);
                    if (batteryEffect is not null)
                        effects.Add(batteryEffect);
                    else
                        needs -= drain;
                }
            }
            if (needs == 0)
                effects.AddRange((modulesById[id] as PoweredModule)!.Supply(power, TotalWeight));
        }
        if (remaining == 0)
            return [.. effects];
        if (batteryCount == 0)
        {
            foreach (var (reactor, excessPower) in DistributeEvenly(remaining, reactors))
            {
                effects.Add(new PowerCannotBeStoredEffect(reactor.Id, excessPower));
            }

            return [.. effects];
        }

        foreach (var (battery, store) in DistributeEvenly(remaining, batteries))
        {
            var batteryEffect = battery.Store(store);
            if (batteryEffect is not null)
                effects.Add(batteryEffect);
        }

        return [.. effects];
    }

    private static IEnumerable<(T Recipient, int Amount)> DistributeEvenly<T>(
        int amount,
        IReadOnlyList<T> recipients)
    {
        var amountPerRecipient = amount / recipients.Count;
        var recipientsWithOneMore = amount % recipients.Count;
        for (var index = 0; index < recipients.Count; index++)
        {
            var recipientAmount = amountPerRecipient;
            if (index < recipientsWithOneMore)
                recipientAmount++;

            if (recipientAmount > 0)
                yield return (recipients[index], recipientAmount);
        }
    }

    public void Attach()
    {
        if (isAttached)
        {
            throw new InvalidOperationException("A module rack can only be attached to one bot.");
        }

        isAttached = true;
    }
}
