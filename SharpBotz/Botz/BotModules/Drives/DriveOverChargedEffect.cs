namespace SharpBotz.Botz.BotModules.Drives;

public record DriveOverChargedEffect(ModuleId Id, int ExcessPower) : ModuleEffect(Id);