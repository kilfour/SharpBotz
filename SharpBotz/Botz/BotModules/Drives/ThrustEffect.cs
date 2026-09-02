namespace SharpBotz.Botz.BotModules.Drives;

public record ThrustEffect(ModuleId Source, int Speed) : ModuleEffect(Source);
