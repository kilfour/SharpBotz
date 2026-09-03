using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges.C_LongShot;

public class LongShotBot() : Bot(new LongShotBrain(), ModuleRack.Create())
{
    public class LongShotBrain : BotBrain
    {
        protected override PowerPlan RoutePower(
            ModuleControl modules,
            BotObservation observation) =>
            PowerPlan.Empty;
    }
}
