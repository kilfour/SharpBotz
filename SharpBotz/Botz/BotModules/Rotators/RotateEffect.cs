namespace SharpBotz.Botz.BotModules.Rotators;

public record RotateEffect(ModuleId Id, Rotation Rotation)
    : ModuleEffect(Id);
