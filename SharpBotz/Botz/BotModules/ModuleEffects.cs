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
        RotatorEffects = [.. effects.Where(a => a is RotateEffect)];
        DriveEffects = [.. effects.Where(a => a is DriveEffect)];
        MeleeEffects = [.. effects.Where(a => a is MeleeEffect)];
        RangedEffects = [.. effects.Where(a => a is RangedEffect)];
        ScannerEffects = [.. effects.Where(a => a is ScanEffect)];
    }

    public readonly ModuleEffect[] ReactorEffects { get; private init; }
    public readonly ModuleEffect[] RotatorEffects { get; private init; }
    public readonly ModuleEffect[] DriveEffects { get; private init; }
    public readonly ModuleEffect[] MeleeEffects { get; private init; }
    public readonly ModuleEffect[] RangedEffects { get; private init; }
    public readonly ModuleEffect[] ScannerEffects { get; private init; }
}
