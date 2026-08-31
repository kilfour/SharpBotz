namespace SharpBotz.Botz.BotModules.Batteries;

public record BatteryInfo(
    ModuleId Id,
    int Weight,
    int Capacity,
    int Charge)
    : ModuleInfo(Id);