using SharpBotz.Botz.BotModules.Batteries;
using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.MeleeWeapons;
using SharpBotz.Botz.BotModules.RangedWeapons;
using SharpBotz.Botz.BotModules.Reactors;
using SharpBotz.Botz.BotModules.Rotators;
using SharpBotz.Botz.BotModules.Scanners;

namespace SharpBotz.Botz.BotModules;

public readonly record struct ModuleEffects
{
    public static ModuleEffects From(ModuleEffect[] effects)
        => new(effects);

    private ModuleEffects(ModuleEffect[] effects)
    {
        ReactorEffects = [.. effects.Where(a => a is ReactorOverLoadedEffect)];
        RotatorEffects = [.. effects.Where(a => a is RotateEffect || a is RotatorOverChargedEffect)];
        DriveEffects = [.. effects.Where(a => a is DriveEffect || a is DriveOverChargedEffect)];
        MeleeEffects = [.. effects.Where(a => a is MeleeEffect || a is MeleeOverChargedEffect)];
        RangedEffects = [.. effects.Where(a => a is RangedEffect || a is RangedOverChargedEffect)];
        BatteryEffects = [.. effects.Where(a =>
            a is PowerCannotBeStoredEffect ||
            a is BatteryDrainedEffect ||
            a is BatteryOverChargedEffect)];
        ScannerEffects = [.. effects.Where(a => a is ScanEffect || a is ScannerOverChargedEffect)];
    }

    public readonly ModuleEffect[] ReactorEffects { get; private init; }
    public readonly ModuleEffect[] RotatorEffects { get; private init; }
    public readonly ModuleEffect[] DriveEffects { get; private init; }
    public readonly ModuleEffect[] MeleeEffects { get; private init; }
    public readonly ModuleEffect[] RangedEffects { get; private init; }
    public readonly ModuleEffect[] BatteryEffects { get; private init; }
    public readonly ModuleEffect[] ScannerEffects { get; private init; }
}
