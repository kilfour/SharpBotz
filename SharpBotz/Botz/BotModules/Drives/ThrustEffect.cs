namespace SharpBotz.Botz.BotModules.Drives;

public record ThrustEffect(
    ModuleId Source,
    int Thrust,
    int MaximumSpeed)
    : ModuleEffect(Source);