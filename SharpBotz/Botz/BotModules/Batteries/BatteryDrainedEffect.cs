namespace SharpBotz.Botz.BotModules.Batteries;

public record BatteryDrainedEffect(ModuleId Id, int ExcessPower) : ModuleEffect(Id);
