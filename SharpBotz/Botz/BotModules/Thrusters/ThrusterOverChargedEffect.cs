namespace SharpBotz.Botz.BotModules.Thrusters;

public record ThrusterOverChargedEffect(ModuleId Id, int ExcessPower) : ModuleEffect(Id);