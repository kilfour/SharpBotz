namespace SharpBotz.Botz.BotModules;

public record PowerGeneration(ModuleId ModuleId, int Power) : PowerModuleIntent(ModuleId);
