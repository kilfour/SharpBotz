namespace SharpBotz.Botz.BotModules.RangedWeapons;

public record RangedEffect(ModuleId Id, int Range, int Damage) : ModuleEffect(Id);
