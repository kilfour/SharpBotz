using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.A_DeadAhead;

public class AheadBot() : Bot(new AheadBrain(), ModuleRack.Create())
{
    public class AheadBrain : BotBrain
    {
        protected override PowerPlan RoutePower(ModuleControl modules, BotObservation observation) =>
            PowerPlan.Empty;
    }
}

