using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Botz.BotModules;


public class ModuleRack
{
    public const int ChassisWeight = 10;
    private readonly BotModule[] modules;
    private readonly IReadOnlyDictionary<ModuleId, BotModule> modulesById;
    // private readonly PoweredModule[] poweredModules;
    private readonly Battery[] batteries;
    private readonly Reactor[] reactors;
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
        // poweredModules = this.modules.OfType<PoweredModule>().ToArray();
        batteries = [.. this.modules.OfType<Battery>()];
        reactors = [.. this.modules.OfType<Reactor>()];
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

    public int BatteryCapacity => batteries.Sum(battery => battery.Capacity);

    public int ReactorOutput => reactors.Sum(reactor => reactor.CurrentOutput);

    public int MaximumReactorOutput => reactors.Sum(reactor => reactor.MaximumOutput);

    public IReadOnlyList<ModuleInfo> Modules =>
        [.. modules.Select(module => module.GetInfo(TotalWeight))];

    // public bool TryValidate(PowerPlan plan, out ValidatedPowerPlan validated)
    // {
    //     var allocatedModules = new HashSet<ModuleId>();
    //     var allocations = new List<(PoweredModule Module, int Power)>();
    //     var reactorOutputs = new List<(Reactor Reactor, int Output)>();
    //     long totalPower = 0;

    //     foreach (var allocation in plan.Allocations)
    //     {
    //         if (!modulesById.TryGetValue(allocation.Module, out var installedModule) ||
    //             !allocatedModules.Add(allocation.Module) ||
    //             allocation.Power < 0)
    //         {
    //             validated = default;
    //             return false;
    //         }

    //         if (installedModule is Reactor reactor)
    //         {
    //             if (allocation.Power > reactor.MaximumOutput)
    //             {
    //                 validated = default;
    //                 return false;
    //             }

    //             reactorOutputs.Add((reactor, allocation.Power));
    //             continue;
    //         }

    //         if (installedModule is not PoweredModule module ||
    //             allocation.Power > module.GetLoadedMaximumPower(TotalWeight))
    //         {
    //             validated = default;
    //             return false;
    //         }

    //         totalPower += allocation.Power;
    //         if (totalPower > int.MaxValue)
    //         {
    //             validated = default;
    //             return false;
    //         }

    //         allocations.Add((module, allocation.Power));
    //     }

    //     validated = new((int)totalPower, allocations, reactorOutputs);
    //     return true;
    // }

    // public void SetReactorOutputs(ValidatedPowerPlan plan)
    // {
    //     foreach (var reactor in reactors)
    //     {
    //         reactor.SetOutput(reactor.MaximumOutput);
    //     }

    //     foreach (var (reactor, output) in plan.ReactorOutputs)
    //     {
    //         reactor.SetOutput(output);
    //     }
    // }

    public bool TryGeneratePower()
    {
        var output = reactors.Sum(reactor => reactor.CurrentOutput);
        var availableCapacity = batteries.Sum(battery => battery.AvailableCapacity);

        if (output > availableCapacity)
        {
            EmptyBatteries();
            return false;
        }

        var remaining = output;
        foreach (var battery in batteries)
        {
            var stored = Math.Min(remaining, battery.AvailableCapacity);
            battery.Store(stored);
            remaining -= stored;
        }

        return true;
    }

    public bool TryConsumePower(int amount)
    {
        if (amount > BatteryLevel)
        {
            EmptyBatteries();
            return false;
        }

        var remaining = amount;
        foreach (var battery in batteries)
        {
            remaining -= battery.Drain(remaining);
        }

        return true;
    }

    public void Attach()
    {
        if (isAttached)
        {
            throw new InvalidOperationException("A module rack can only be attached to one bot.");
        }

        isAttached = true;
    }

    // public IReadOnlyList<ModuleEffect> Apply(ValidatedPowerPlan plan)
    // {
    //     Disconnect();

    //     foreach (var (module, power) in plan.Allocations)
    //     {
    //         module.Supply(power);
    //     }

    //     return poweredModules
    //         .SelectMany(module => module.GetEffects(TotalWeight))
    //         .ToArray();
    // }

    // public void Disconnect()
    // {
    //     foreach (var module in poweredModules)
    //     {
    //         module.Disconnect();
    //     }
    // }

    private void EmptyBatteries()
    {
        foreach (var battery in batteries)
        {
            battery.Empty();
        }
    }
}

// public readonly record struct ValidatedPowerPlan(
//     int TotalPower,
//     IReadOnlyList<(PoweredModule Module, int Power)> Allocations,
//     IReadOnlyList<(Reactor Reactor, int Output)> ReactorOutputs);
