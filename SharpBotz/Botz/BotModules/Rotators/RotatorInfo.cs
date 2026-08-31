namespace SharpBotz.Botz.BotModules.Rotators;


public abstract record RotatorInfo(
    ModuleId Id,
    int Weight,
    int ActivationPower,
    int MaximumPower)
    : PoweredModuleInfo(Id/*, Weight, ActivationPower, MaximumPower, CurrentPower */);

public sealed record LeftRotatorInfo(
    ModuleId Id,
    int Weight,
    int ActivationPower,
    int MaximumPower)
    : RotatorInfo(Id, Weight, ActivationPower, MaximumPower);

public sealed record RightRotatorInfo(
    ModuleId Id,
    int Weight,
    int ActivationPower,
    int MaximumPower)
    : RotatorInfo(Id, Weight, ActivationPower, MaximumPower);
