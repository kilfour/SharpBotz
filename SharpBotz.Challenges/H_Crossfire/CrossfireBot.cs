using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.H_Crossfire;

public class CrossfireBot() : Bot(new CrossfireBrain(), ModuleRack.Create())
{
    public class CrossfireBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
