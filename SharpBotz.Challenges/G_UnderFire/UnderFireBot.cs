using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.G_UnderFire;

public class UnderFireBot() : Bot(new UnderFireBrain(), ModuleRack.Create())
{
    public class UnderFireBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
