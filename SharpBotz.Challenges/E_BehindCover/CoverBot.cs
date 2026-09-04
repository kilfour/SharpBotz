using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.E_BehindCover;

public class CoverBot() : Bot(nameof(CoverBot), new CoverBrain(), ModuleRack.Create())
{
    public class CoverBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
