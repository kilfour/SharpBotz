namespace SharpBotz.Botz.BotModules;

public record PowerCannotBeStoredEffect(ModuleId Id, int ExcessPower) : ModuleEffect(Id);
