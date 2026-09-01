using SharpBotz.Botz.BotModules;

namespace SharpBotz.Worlds;

// public abstract class EffectHandler(Func<ModuleEffects, ModuleEffect[]> getEffects, Action<ModuleEffect> handleEffect)
// {
//     public abstract void Handle(BotStateEffect[] botEffects);
// }


public record BotStateEffect(BotState BotState, ModuleEffects Effects);