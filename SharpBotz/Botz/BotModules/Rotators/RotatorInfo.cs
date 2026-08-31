namespace SharpBotz.Botz.BotModules.Rotators;


public abstract record RotatorInfo(
    ModuleId Id,
    int Weight,
    int ActivationPower,
    int MaximumPower,
    int CurrentPower)
    : PoweredModuleInfo(Id/*, Weight, ActivationPower, MaximumPower, CurrentPower */);

public sealed record LeftRotatorInfo(
    ModuleId Id,
    int Weight,
    int ActivationPower,
    int MaximumPower,
    int CurrentPower)
    : RotatorInfo(Id, Weight, ActivationPower, MaximumPower, CurrentPower);

public sealed record RightRotatorInfo(
    ModuleId Id,
    int Weight,
    int ActivationPower,
    int MaximumPower,
    int CurrentPower)
    : RotatorInfo(Id, Weight, ActivationPower, MaximumPower, CurrentPower);
