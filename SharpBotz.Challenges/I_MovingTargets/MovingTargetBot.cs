using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.I_MovingTargets;

public class MovingTargetBot() : Bot(nameof(MovingTargetBot), new MovingTargetBrain(), ModuleRack.Create())
{
    public class MovingTargetBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
