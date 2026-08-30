namespace SharpBotz.Botz.BotModules;

public abstract record ModuleEffect(ModuleId Source);



public record RotateEffect(ModuleId Source, Rotation Rotation)
    : ModuleEffect(Source);

public record MeleeEffect(ModuleId Source, int Damage)
    : ModuleEffect(Source);

public record RangedEffect(ModuleId Source, int Range, int Damage)
    : ModuleEffect(Source);

public record ScanEffect(ModuleId Source, int Range)
    : ModuleEffect(Source);

public enum Rotation
{
    Left,
    Right,
}
