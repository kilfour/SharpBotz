using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.J_TrialByFire;

public class TrialBot() : Bot(new TrialBrain(), ModuleRack.Create())
{
    public class TrialBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
