using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Challenges;

public class DummyBot() : Bot(nameof(DummyBot), new DummyBrain(), ModuleRack.Create())
{
    public class DummyBrain : BotBrain
    {
        protected override PowerPlan RoutePower(ModuleControl modules, BotObservation observation) =>
            PowerPlan.Empty;
    }
}
