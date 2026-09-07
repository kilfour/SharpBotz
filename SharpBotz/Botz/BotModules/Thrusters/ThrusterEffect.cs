namespace SharpBotz.Botz.BotModules.Thrusters;

public record ThrusterEffect(ModuleId Source, int Speed) : ModuleEffect(Source);
