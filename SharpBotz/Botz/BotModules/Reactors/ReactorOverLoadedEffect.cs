namespace SharpBotz.Botz.BotModules.Reactors;

public record ReactorOverLoadedEffect(ModuleId Id, int ExcessPower) : ModuleEffect(Id);