using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.B_DifferentRoutes;

public class RouteBot() : Bot(new RouteBrain(), ModuleRack.Create())
{
    public class RouteBrain : BotBrain
    {
        protected override PowerPlan RoutePower(ModuleControl modules, BotObservation observation) =>
            PowerPlan.Empty;
    }
}

