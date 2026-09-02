namespace SharpBotz.Botz.BotModules.Drives;

public record DriveEffect(ModuleId Source, int Speed) : ModuleEffect(Source);
