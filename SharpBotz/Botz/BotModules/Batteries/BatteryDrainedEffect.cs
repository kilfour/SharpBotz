namespace SharpBotz.Botz.BotModules.Batteries;

public record BatteryDrainedEffect(ModuleId Id) : ModuleEffect(Id);


public record BatteryOverChargedEffect(ModuleId Id) : ModuleEffect(Id);