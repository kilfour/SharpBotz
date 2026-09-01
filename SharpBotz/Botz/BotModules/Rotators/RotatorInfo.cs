namespace SharpBotz.Botz.BotModules.Rotators;


public abstract record RotatorInfo(ModuleId Id)
    : PoweredModuleInfo(Id);

public record LeftRotatorInfo(ModuleId Id)
    : RotatorInfo(Id);

public record RightRotatorInfo(ModuleId Id)
    : RotatorInfo(Id);
