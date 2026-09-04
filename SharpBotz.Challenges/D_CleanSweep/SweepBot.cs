using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.D_CleanSweep;

public class SweepBot() : Bot(nameof(SweepBot), new SweepBrain(), ModuleRack.Create())
{
    public class SweepBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
