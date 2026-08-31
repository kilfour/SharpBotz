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
            if (power > reactors.Single(a => a.Id == id).MaximumOutput)
                effects.Add(new ReactorOverLoadedEffect(id));
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
            var needsPerBattery = needs / batteryCount;
            var firstNeedBattery = needsPerBattery + needs % batteryCount;
            var firstNeed = true;
            foreach (var battery in batteries)
            {
                var drain = firstNeed ? firstNeedBattery : needsPerBattery;
                firstNeed = false;
                var batteryEffect = battery.Drain(drain);
                if (batteryEffect is not null)
                    effects.Add(batteryEffect);
                else
                    needs -= drain;
            }
            if (needs == 0)
                effects.AddRange((modulesById[id] as PoweredModule)!.Supply(power, TotalWeight));
        }

        var storePerBattery = remaining / batteryCount;
        var firstStoreBattery = storePerBattery + remaining % batteryCount;
        var firstStore = true;
        foreach (var battery in batteries)
        {
            var store = firstStore ? firstStoreBattery : storePerBattery;
            firstStore = false;
            var batteryEffect = battery.Store(store);
            if (batteryEffect is not null)
                effects.Add(batteryEffect);
        }

        return [.. effects];
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
