using SharpBotz.Botz.BotModules.Drives;
using SharpBotz.Botz.BotModules.Reactors;

namespace SharpBotz.Botz.BotModules;

public readonly record struct ModuleEffects
{
    public static ModuleEffects From(ModuleEffect[] effects)
        => new(effects);

    private ModuleEffects(ModuleEffect[] effects)
    {
        ReactorEffects = [.. effects.Where(a => a is ReactorOverLoadedEffect)];
        DriveEffects = [.. effects.Where(a => a is ThrustEffect)];
    }

    public readonly ModuleEffect[] ReactorEffects { get; private init; }
    public readonly ModuleEffect[] DriveEffects { get; private init; }
}
