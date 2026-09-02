namespace SharpBotz.Botz.BotModules.Batteries;

public record BatteryOverChargedEffect(ModuleId Id, int ExcessPower) : ModuleEffect(Id);