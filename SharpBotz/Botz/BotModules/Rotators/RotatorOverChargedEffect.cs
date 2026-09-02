

namespace SharpBotz.Botz.BotModules.Rotators;

public record RotatorOverChargedEffect(ModuleId Id, int ExcessPower) : ModuleEffect(Id);