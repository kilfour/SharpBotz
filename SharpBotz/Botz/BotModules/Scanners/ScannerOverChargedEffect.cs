namespace SharpBotz.Botz.BotModules.Scanners;

public record ScannerOverChargedEffect(ModuleId Id, int ExcessPower) : ModuleEffect(Id);
