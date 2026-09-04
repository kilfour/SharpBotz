using SharpBotz.Botz;
using SharpBotz.Botz.BotModules;

namespace SharpBotz.Tests;

public class DummyBrain : BotBrain
{
    protected override PowerPlan RoutePower(ModuleControl modules, BotObservation observation) =>
        PowerPlan.Empty;
}

public class DummyBot() : Bot(nameof(DummyBot), new DummyBrain(), ModuleRack.Create());
